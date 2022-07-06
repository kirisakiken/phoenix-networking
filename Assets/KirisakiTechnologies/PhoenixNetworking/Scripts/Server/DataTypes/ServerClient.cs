using System;
using System.Net;
using System.Net.Sockets;

using JetBrains.Annotations;

using KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules;

using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.DataTypes
{
    public class ServerClient : IServerClient
    {
        #region Constructors

        public ServerClient([NotNull] IServerModule serverModule, int id) // TODO: remove module from constructor when dependencies are refactored
        {
            Id = id;
            ServerTcp = new ServerTcp(serverModule, id, DataBufferSize);
            ServerUdp = new ServerUdp(serverModule, id);
        }

        #endregion

        #region IServerClient Implementation

        public int Id { get; }
        public string Name { get; set; }
        public IServerTcp ServerTcp { get; }
        public IServerUdp ServerUdp { get; }

        #endregion

        #region Private

        private const int DataBufferSize = 4096;

        #endregion
    }

    public class ServerTcp : IServerTcp, IDisposable
    {
        #region Constructors

        public ServerTcp([NotNull] IServerModule serverModule, int id, int dataBufferSize) // TODO: remove module from constructor when dependencies are refactored
        {
            _ServerModule = serverModule;
            Id = id;
            DataBufferSize = dataBufferSize;
        }

        #endregion

        #region IServerTcp Implementation

        public bool IsConnected => Socket is { Connected: true };

        public int Id { get; }
        public TcpClient Socket { get; private set; }
        
        public int DataBufferSize { get; }

        public void Connect(TcpClient socket)
        {
            Socket = socket ?? throw new ArgumentNullException(nameof(socket));
            Socket.ReceiveBufferSize = DataBufferSize;
            Socket.SendBufferSize = DataBufferSize;

            _Stream = Socket.GetStream();

            _ReceivedData = new Packet();
            _ReceiveBuffer = new byte[DataBufferSize];

            _Stream.BeginRead(_ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }

        // TODO: rename to e.g. WriteStreamData/WriteData/WritePacket/WriteAndSendData
        public void SendData(Packet packet)
        {
            try
            {
                if (Socket == null)
                    return;

                if (!IsConnected) // TODO: experiment with this statement
                    Debug.LogError($"Can not send data to client which is not connected: clientId: {Id}");

                _Stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null); // TODO: refactor packet file
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Unable to send data: {e.Message}");
            }
        }

        #endregion

        #region IDisposable Implementation

        // TODO: overall dispose check after full implementation
        public void Dispose()
        {
            _Stream?.Dispose();
            _ReceivedData?.Dispose();
            Socket?.Dispose();
        }

        #endregion

        #region Public

        

        #endregion

        #region Private

        private NetworkStream _Stream;

        private byte[] _ReceiveBuffer;
        private Packet _ReceivedData; // TODO: refactor packet file

        private IServerModule _ServerModule; // TODO: remove module from constructor when dependencies are refactored

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                var byteLength = _Stream.EndRead(result);
                if (byteLength <= 0)
                    return;

                var data = new byte[byteLength];
                Array.Copy(_ReceiveBuffer, data, byteLength);

                _ReceivedData.Reset(HandleData(data)); // TODO: refactor packet file

                _Stream.BeginRead(_ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private bool HandleData(byte[] data) // TODO: this method does not belong into lower data class like this
        {
            var packetLength = 0;

            _ReceivedData.SetBytes(data); // TODO: refactor packet file

            if (_ReceivedData.UnreadLength() >= 4)
            {
                packetLength = _ReceivedData.ReadInt();
                if (packetLength <= 0)
                    return true;
            }

            // TODO: refactor the while loop below
            while (packetLength > 0 && packetLength <= _ReceivedData.UnreadLength())
            {
                var packetBytes = _ReceivedData.ReadBytes(packetLength);
                
                StaticThreadModule.ExecuteOnMainThread(() =>
                {
                    using (var packet = new Packet(packetBytes))
                    {
                        var packetId = packet.ReadInt();
                        // TODO: refactor below
                        _ServerModule.PacketHandlers[packetId](Id, packet);
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

        #endregion
    }

    public class ServerUdp : IServerUdp, IDisposable
    {
        #region Constructors

        public ServerUdp([NotNull] IServerModule serverModule, int id)
        {
            _ServeModule = serverModule ?? throw new ArgumentNullException(nameof(serverModule));
            Id = id;
        }

        #endregion

        #region IServerUdp Implementation

        public int Id { get; }
        public IPEndPoint EndPoint { get; private set; }

        public void Connect(IPEndPoint endPoint)
        {
            EndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
        }

        public void SendData(Packet packet)
        {
            _ServeModule.SendUdpData(EndPoint, packet);
        }

        public void HandleData(Packet packet)
        {
            var packetLength = packet.ReadInt();
            var packetBytes = packet.ReadBytes(packetLength);

            StaticThreadModule.ExecuteOnMainThread(() =>
            {
                using (var packet = new Packet(packetBytes))
                {
                    var packetId = packet.ReadInt();
                    _ServeModule.PacketHandlers[packetId](Id, packet);
                }
            });
        }

        #endregion

        #region IDisposable Implementation

        // TODO: do overall dispose check after full implementation
        public void Dispose()
        {
            
        }

        #endregion

        #region Private

        private IServerModule _ServeModule;

        #endregion
    }
}