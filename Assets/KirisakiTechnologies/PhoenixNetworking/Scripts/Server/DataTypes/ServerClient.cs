using System;
using System.Net.Sockets;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.DataTypes
{
    public class ServerClient : IServerClient
    {
        #region Constructors

        public ServerClient(int id, IServerModule serverModule) // TODO: remove module from constructor when dependencies are refactored
        {
            Id = id;
            ServerTcp = new ServerTcp(id, DataBufferSize, serverModule);
        }

        #endregion

        #region IServerClient Implementation

        public int Id { get; }
        public IServerTcp ServerTcp { get; }

        #endregion

        #region Public

        

        #endregion

        #region Private

        private const int DataBufferSize = 4096;

        #endregion
    }

    public class ServerTcp : IServerTcp
    {
        #region Constructors

        public ServerTcp(int id, int dataBufferSize, IServerModule serverModule) // TODO: remove module from constructor when dependencies are refactored
        {
            Id = id;
            DataBufferSize = dataBufferSize;
            _ServerModule = serverModule;
        }

        #endregion

        #region IServerTcp Implementation

        public int Id { get; }
        public TcpClient Socket { get; private set; }
        
        public int DataBufferSize { get; }

        public void Connect(TcpClient socket)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = DataBufferSize;
            Socket.SendBufferSize = DataBufferSize;

            _Stream = Socket.GetStream();

            _ReceiveBuffer = new byte[DataBufferSize];
            _ReceivedData = new Packet(); // TODO: refactor packet file

            _Stream.BeginRead(_ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }

        // TODO: rename to e.g. WriteStreamData/WriteData/WritePacket
        public void SendData(Packet packet)
        {
            try
            {
                if (Socket == null)
                    return;

                _Stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null); // TODO: refactor packet file
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Unable to send data: {e.Message}");
            }
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
                
                ThreadManager.ExecuteOnMainThread(() =>
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
}