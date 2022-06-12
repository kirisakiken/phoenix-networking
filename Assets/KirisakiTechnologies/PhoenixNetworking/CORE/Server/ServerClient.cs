using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
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

                _ReceiveBuffer = new byte[DataBufferSize];

                // start reading data
                _Stream.BeginRead(_ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
                
                // TODO: send welcome packet
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
                    Debug.LogError($"Error receiving ServerTcp data: ${e.Message}");
                    // TODO: disconnect
                    throw;
                }
            }
        }
    }
}