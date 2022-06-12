using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.CORE.Client
{
    public class Client : MonoBehaviour
    {
        public static int DataBufferSize = 4096;
        public static string Ip = "127.0.0.1";
        public static uint Port = 26950;

        public uint Id = 1;

        public ClientTcp Tcp;

        private void Start()
        {
            Tcp = new ClientTcp();
            ConnectToServer();
        }

        public void ConnectToServer()
        {
            Tcp.Connect();
        }
        
        public class ClientTcp
        {
            public TcpClient Socket { get; private set; }
            private NetworkStream _Stream;
            private byte[] _ReceiveBuffer;

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

            private void ClientConnectCallback(IAsyncResult result)
            {
                Socket.EndConnect(result);

                if (!Socket.Connected)
                    return;

                _Stream = Socket.GetStream();

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
                    
                    // Continue reading
                    _Stream.BeginRead(_ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
                }
                catch (Exception e)
                {
                    // TODO: disconnect
                }
            }
        }
    }

    
}
