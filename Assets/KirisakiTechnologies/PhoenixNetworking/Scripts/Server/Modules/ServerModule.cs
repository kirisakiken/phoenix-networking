using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Server.DataTypes;

using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules
{
    public class ServerModule : GameModuleBaseMono, IServerModule
    {
        #region IServerModule Implementation

        public event NetworkEvent OnClientConnected;
        public event PacketEvent OnClientConnectionHandshakeCompleted;
        public event PacketEvent OnClientTcpMessagePayloadReceived;
        public event PacketEvent OnClientUdpPayloadReceived;
        public event PacketEvent OnUdpClientInputTickReceived; 

        public IReadOnlyDictionary<int, IServerClient> Clients => _Clients;
        public IReadOnlyDictionary<int, PacketHandler> PacketHandlers => _PacketHandlers;

        public void SendUdpData(IPEndPoint clientEndPoint, Packet packet)
        {
            try
            {
                if (clientEndPoint != null)
                    _UdpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error sending udp data : {e.Message}");
            }
        }

        #endregion

        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            InitializeServerData();
            InitializeTcp();
            InitializeUdp();

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Public

        

        #endregion

        #region Private

        [SerializeField]
        [Min(0)]
        private int _MaxClientCount = 10;

        [SerializeField]
        [Min(0)]
        private int _Port = 26950;

        private TcpListener _TcpListener;
        private UdpClient _UdpListener;

        private readonly Dictionary<int, IServerClient> _Clients = new Dictionary<int, IServerClient>();
        private readonly Dictionary<int, PacketHandler> _PacketHandlers = new Dictionary<int, PacketHandler>();

        private void InitializeServerData()
        {
            for (var i = 1; i <= _MaxClientCount; ++i)
                _Clients.Add(i, new ServerClient(this, i));

            _PacketHandlers.Add((int) ClientPackets.ConnectReceived, ClientConnected);
            _PacketHandlers.Add((int) ClientPackets.TcpMessagePayloadReceived, ClientTcpMessagePayloadReceived);
            _PacketHandlers.Add((int) ClientPackets.UdpTestReceive, ClientUdpPayloadReceived);
            _PacketHandlers.Add((int) ClientPackets.UdpClientInputTickReceived, UdpClientInputTickReceived);

            Debug.Log("ServerModule: Initialized Clients Collection");
            Debug.Log("ServerModule: Initialized Server Packet Handlers");
        }

        private void ClientConnected(int clientId, Packet packet) => OnClientConnectionHandshakeCompleted?.Invoke(clientId, packet);

        private void ClientTcpMessagePayloadReceived(int clientId, Packet packet) => OnClientTcpMessagePayloadReceived?.Invoke(clientId, packet);

        private void ClientUdpPayloadReceived(int clientId, Packet packet) => OnClientUdpPayloadReceived?.Invoke(clientId, packet);

        private void UdpClientInputTickReceived(int clientId, Packet packet) => OnUdpClientInputTickReceived?.Invoke(clientId, packet);
        private void InitializeTcp()
        {
            _TcpListener = new TcpListener(IPAddress.Any, _Port);
            _TcpListener.Start();

            _TcpListener.BeginAcceptTcpClient(TcpClientConnectCallback, null);
            
            Debug.Log($"TCP Server started on port: {_Port}");
        }

        private void InitializeUdp()
        {
            _UdpListener = new UdpClient(_Port);
            _UdpListener.BeginReceive(UdpReceiveCallback, null);

            Debug.Log($"UDP Server has been initialized");
        }

        private void TcpClientConnectCallback(IAsyncResult result)
        {
            var client = _TcpListener.EndAcceptTcpClient(result);
            _TcpListener.BeginAcceptTcpClient(TcpClientConnectCallback, null);

            Debug.Log($"Incoming connection from: {client.Client.RemoteEndPoint}");

            // adds connected client to Clients collection (refactorable)
            for (var i = 1; i <= _MaxClientCount; ++i)
            {
                if (Clients[i].ServerTcp.Socket != null)
                    continue;

                Clients[i].ServerTcp.Connect(client);
                OnClientConnected?.Invoke(Clients[i].Id);

                return;
            }

            Debug.LogError($"{client.Client.RemoteEndPoint} failed to connect. Server is full!");
        }

        private void UdpReceiveCallback(IAsyncResult result)
        {
            try
            {
                var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                var data = _UdpListener.EndReceive(result, ref clientEndPoint);
                _UdpListener.BeginReceive(UdpReceiveCallback, null);

                if (data.Length < 4)
                    return;

                using (var packet = new Packet(data))
                {
                    var clientId = packet.ReadInt();

                    if (clientId == 0) // prevents server crash
                        return;

                    // means new client connection if endpoint is null
                    // if (!Clients.ContainsKey(clientId))
                    //     throw new KeyNotFoundException($"Unable to find client key with ID: {clientId} in collection: {nameof(Clients)}");
                    if (Clients[clientId].ServerUdp.EndPoint == null)
                    {
                        Clients[clientId].ServerUdp.Connect(clientEndPoint);
                        return;
                    }

                    // preventing players impersonating other players by checking id
                    if (Clients[clientId].ServerUdp.EndPoint.ToString() == clientEndPoint.ToString())
                    {
                        Clients[clientId].ServerUdp.HandleData(packet);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error during udp receive callback : {e.Message}");
            }
        }

        #endregion

        #region MonoBehaviour Methods

        private void OnApplicationQuit()
        {
            _TcpListener.Stop();
            _UdpListener.Close();
        }

        #endregion
    }
}
