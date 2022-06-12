using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using KirisakiTechnologies.PhoenixNetworking.CORE.Server;
using UnityEditor.Sprites;
using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.CORE.Client
{
    /// <summary>
    ///     Represents a client instance that being stored in Server
    /// </summary>
    /// TODO: convert to interface
    /// TODO: rename to ClientModule maybe?
    public class ServerClient
    {
        public static int DataBufferSize = 4096;
        public uint Id { get; }
        public ServerTcp Tcp { get; }

        public ServerClient(uint id)
        {
            Id = id;
            Tcp = new ServerTcp(id);
        }

        public class ServerTcp
        {
            public TcpClient Socket;
            private uint Id;

            private NetworkStream _Stream;
            private byte[] _ReceiveBuffer;

            private Packet _ReceivedData;

            private string OnConnectMessage => $"Welcome to the server. Client: Guest[{Id}]";

            public ServerTcp(uint id)
            {
                Id = id;
            }

            public void Connect(TcpClient socket)
            {
                Socket = socket;
                Socket.ReceiveBufferSize = DataBufferSize;
                Socket.SendBufferSize = DataBufferSize;

                _Stream = Socket.GetStream();

                _ReceivedData = new Packet();
                _ReceiveBuffer = new byte[DataBufferSize];

                // start reading data
                _Stream.BeginRead(_ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
                
                // send on connected message to connected client
                ServerSend.Welcome((int) Id, OnConnectMessage);
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
                    Console.WriteLine(e);
                    throw;
                }
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
                    
                    // handle data
                    _ReceivedData.Reset(HandleData(data));
                    
                    // Continue reading
                    _Stream.BeginRead(_ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error receiving ServerTcp data: ${e.Message}");
                    // TODO: disconnect
                    throw;
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
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (var packet = new Packet(packetBytes))
                        {
                            var packetId = packet.ReadInt();
                            Server.Server._PacketHandlers[packetId]((int) Id, packet);
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
    }
}