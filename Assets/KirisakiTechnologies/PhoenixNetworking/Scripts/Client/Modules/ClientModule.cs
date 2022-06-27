using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

using JetBrains.Annotations;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules;
using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules
{
    public class ClientModule : GameModuleBaseMono, IClientModule
    {
        #region IClientModule Implementation

        public event PacketEvent OnClientConnected;
        public event PacketEvent OnClientConnectBroadcastReceived;
        public event PacketEvent OnClientTcpMessagePayloadReceived;

        public int Id { get; set; }
        public IClientTcp Tcp { get; private set; }

        public IReadOnlyDictionary<int, PacketHandler> PacketHandlers => _PacketHandlers;

        public void ConnectToServer()
        {
            if (Tcp is { IsConnected: true })
                return;

            Tcp ??= new ClientTcp(this, _Ip, _Port, _DataBufferSize, _DataBufferSize);
            Tcp.Connect();
        }

        public void ConnectToServer(string nickName, string ip, uint port)
        {
            if (Tcp is { IsConnected: true })
                return;

            Tcp ??= new ClientTcp(this, ip, port, _DataBufferSize, _DataBufferSize);
            Tcp.Connect();
        }

        public void DisconnectFromServer()
        {
            Tcp?.Disconnect();
        }

        #endregion

        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            InitializeClientTcpData();

            return Task.CompletedTask;
        }

        #endregion

        #region Private

        [SerializeField]
        private int _DataBufferSize = 4096;

        [SerializeField]
        private string _Ip = "127.0.0.1";

        [SerializeField]
        private uint _Port = 26950;

        private readonly Dictionary<int, PacketHandler> _PacketHandlers = new Dictionary<int, PacketHandler>();

        private void InitializeClientTcpData()
        {
            // TODO: find better way to declare message recipes
            _PacketHandlers.Add((int) ServerPackets.ClientConnected, ClientConnected);
            _PacketHandlers.Add((int) ServerPackets.ConnectedClientBroadcast, ClientConnectedBroadcastReceived);
            _PacketHandlers.Add((int) ServerPackets.TcpMessagePayloadReceived, ClientTcpMessagePayloadReceived);
        }

        private void ClientConnected(Packet packet) => OnClientConnected?.Invoke(packet);

        private void ClientConnectedBroadcastReceived(Packet packet) => OnClientConnectBroadcastReceived?.Invoke(packet);

        private void ClientTcpMessagePayloadReceived(Packet packet) => OnClientTcpMessagePayloadReceived?.Invoke(packet);

        #endregion

        #region MonoBehaviour Methods

        private void OnApplicationQuit()
        {
            DisconnectFromServer();
        }

        #endregion

        #region Nested Types

        // TODO: investigate if this should be seperated into another file e.g. DataTypes/IClientTcp
        // TODO: investigate if entire functionality can be moved into IClientModule?
        public class ClientTcp : IClientTcp, IDisposable
        {
            #region Constructors

            public ClientTcp([NotNull] IClientModule clientModule, string ip, uint port, int receiveBufferSize, int sendBufferSize)
            {
                if (string.IsNullOrEmpty(ip))
                    throw new ArgumentNullException(nameof(ip));

                _ClientModule = clientModule ?? throw new ArgumentNullException(nameof(clientModule));
                _Ip = ip;
                _Port = port;

                Socket = new TcpClient
                {
                    ReceiveBufferSize = receiveBufferSize,
                    SendBufferSize = sendBufferSize,
                };

                _ReceiveBuffer = new byte[receiveBufferSize];
            }

            #endregion

            #region IClientTcp Implementation

            public TcpClient Socket { get; }

            public bool IsConnected => Socket is { Connected: true };

            public void Connect()
            {
                if (Socket == null)
                    throw new NullReferenceException(nameof(Socket));

                if (IsConnected)
                    throw new InvalidOperationException("Connect attempt failed. Already connected!");

                Socket.BeginConnect(_Ip, (int) _Port, ClientConnectCallback, Socket);
            }

            public void Disconnect()
            {
                if (!IsConnected)
                    return;

                Socket.Close();
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if (Socket == null)
                        return;

                    _Stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error sending data to server via TCP: {e.Message}");
                    throw;
                }
            }

            #endregion

            #region IDisposable Implementation

            public void Dispose()
            {
                Disconnect();

                _ClientModule = null;
                _Stream?.Dispose();
                _ReceivedData?.Dispose();
                Socket?.Dispose();
                // TODO: overall dispose check after finishing implementation
            }

            #endregion

            #region Private

            private IClientModule _ClientModule;

            private string _Ip;
            private uint _Port;

            private NetworkStream _Stream;
            private byte[] _ReceiveBuffer;
            private Packet _ReceivedData;

            // TODO: find a way to move this method out of this class
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

                    StaticThreadModule.ExecuteOnMainThread(() =>
                    {
                        using (var packet = new Packet(packetBytes))
                        {
                            var packetId = packet.ReadInt();
                            _ClientModule.PacketHandlers[packetId](packet);
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

            #region Event Handlers

            private void ClientConnectCallback(IAsyncResult result)
            {
                Socket.EndConnect(result);

                if (!Socket.Connected)
                    return;

                _Stream = Socket.GetStream();
                _ReceivedData = new Packet(); // TODO: ?? move this to somewhere else???
                _Stream.BeginRead(_ReceiveBuffer, 0, Socket.ReceiveBufferSize, DataReceiveCallback, null);
            }

            private void DataReceiveCallback(IAsyncResult result)
            {
                try
                {
                    var len = _Stream.EndRead(result);
                    if (len <= 0)
                        return;

                    var data = new byte[len];
                    Array.Copy(_ReceiveBuffer, data, len);

                    _ReceivedData.Reset(HandleData(data));

                    _Stream.BeginRead(_ReceiveBuffer, 0, Socket.ReceiveBufferSize, DataReceiveCallback, null);
                }
                catch (Exception e)
                {
                    // TODO: disconnect
                    Debug.LogWarning($"Error receiving data in callback: {e.Message}");
                }
            }

            #endregion
        }

        #endregion
    }
}