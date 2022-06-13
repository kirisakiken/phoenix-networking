using System;
using System.Collections.Generic;
using System.Net.Sockets;

using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client
{
    public class Client : MonoBehaviour
    {
        public static int DataBufferSize = 4096;
        public static string Ip = "127.0.0.1";
        public static uint Port = 26950;

        public static int Id = 1;
        public static string Name = "Johnny";

        public static ClientTcp Tcp;

        private delegate void PacketHandler(Packet packet);
        private static Dictionary<int, PacketHandler> _PacketHandlers;

        private void Start()
        {
            Tcp = new ClientTcp();
            // ConnectToServer();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
                ConnectToServer();
        }

        public void ConnectToServer()
        {
            // TODO: return if already connected
            
            InitializeClientData();
            Tcp.Connect();
        }

        public class ClientTcp
        {
            public TcpClient Socket { get; private set; }
            private NetworkStream _Stream;
            private byte[] _ReceiveBuffer;

            private Packet _ReceivedData;

            public void Connect()
            {
                Socket = new TcpClient
                {
                    ReceiveBufferSize = DataBufferSize,
                    SendBufferSize = DataBufferSize,
                };

                _ReceiveBuffer = new byte[DataBufferSize];
                Socket.BeginConnect(Ip, (int) Port, ClientConnectCallback, Socket);
                
                Debug.Log($"Client: connected to server. . .");
            }
            
            public void SendData(Packet packet)
            {
                try
                {
                    if (Socket != null)
                    {
                        _Stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error sending data to server via Tcp: ${e.Message}");
                    throw;
                }
            }

            private void ClientConnectCallback(IAsyncResult result)
            {
                Socket.EndConnect(result);

                if (!Socket.Connected)
                    return;

                _Stream = Socket.GetStream();

                _ReceivedData = new Packet();

                _Stream.BeginRead(_ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    // see read results. if less than 0 return
                    var byteLength = _Stream.EndRead(result);
                    if (byteLength <= 0)
                        return;

                    // copy read result to buffer
                    var data = new byte[byteLength];
                    Array.Copy(_ReceiveBuffer, data, byteLength);
                    
                    // TODO: handle data
                    _ReceivedData.Reset(HandleData(data));
                    
                    // Continue reading
                    _Stream.BeginRead(_ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
                }
                catch (Exception e)
                {
                    // TODO: disconnect
                }
            }

            private bool HandleData(byte[] data)
            {
                var packetLength = 0;
                
                _ReceivedData.SetBytes(data);

                if (_ReceivedData.UnreadLength() >= 4)
                {
                    packetLength = _ReceivedData.ReadInt();
                    if (packetLength <= 0)
                        return true;
                }

                while (packetLength > 0 && packetLength <= _ReceivedData.UnreadLength())
                {
                    var packetBytes = _ReceivedData.ReadBytes(packetLength);
                    ThreadManager.ExecuteOnMainThread(() => // TODO: ThreadManager to StaticThreadModule refactor on Client repo
                    {
                        using (var packet = new Packet(packetBytes))
                        {
                            var packetId = packet.ReadInt();
                            _PacketHandlers[packetId](packet);
                        }
                    });

                    packetLength = 0;
                    if (_ReceivedData.UnreadLength() >= 4)
                    {
                        packetLength = _ReceivedData.ReadInt();
                        if (packetLength <= 0)
                            return true;
                    }
                }

                if (packetLength <= 1)
                    return true;

                return false;
            }
        }
        
        private void InitializeClientData()
        {
            _PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ServerPackets.welcome, ClientHandle.Welcome},
            };
            
            Debug.Log("Initialized client packet handlers");
        }
    }

    
}
