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

        public IReadOnlyDictionary<int, IServerClient> Clients => _Clients;
        public IReadOnlyDictionary<int, PacketHandler> PacketHandlers => _PacketHandlers;

        #endregion

        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            InitializeServerData();
            InitializeTcp();

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

        private readonly Dictionary<int, IServerClient> _Clients = new Dictionary<int, IServerClient>();
        private readonly Dictionary<int, PacketHandler> _PacketHandlers = new Dictionary<int, PacketHandler>();

        private void InitializeServerData()
        {
            for (var i = 1; i <= _MaxClientCount; ++i)
                _Clients.Add(i, new DataTypes.ServerClient(i));

            // TODO: refactor below
            _PacketHandlers.Add((int) ClientPackets.welcomeReceived, ServerHandler.WelcomeReceived); // TODO: refactor packet files
            
            Debug.Log("Initialized Clients Collection");
            Debug.Log("Initialized Server Packet Handlers");
        }

        private void InitializeTcp()
        {
            _TcpListener = new TcpListener(IPAddress.Any, _Port);
            _TcpListener.Start();

            _TcpListener.BeginAcceptTcpClient(TcpClientConnectCallback, null);
            
            Debug.Log($"Server started on port: {_Port}");
        }

        // TODO: implement
        private void InitializeUdp()
        {
            throw new NotImplementedException(nameof(InitializeUdp));
        }

        private void TcpClientConnectCallback(IAsyncResult result)
        {
            var client = _TcpListener.EndAcceptTcpClient(result);
            _TcpListener.BeginAcceptTcpClient(TcpClientConnectCallback, null);
            
            Debug.Log($"Incoming connection from: {client.Client.RemoteEndPoint}");

            for (var i = 1; i <= _MaxClientCount; ++i)
            {
                if (Clients[i].ServerTcp.Socket == null)
                {
                    Clients[i].ServerTcp.Connect(client);
                    return;
                }
            }
            
            Debug.LogError($"{client.Client.RemoteEndPoint} failed to connect. Server is full!");
        }

        #endregion

        #region MonoBehaviour Methods

        private void OnApplicationQuit()
        {
            _TcpListener.Stop();
        }

        #endregion
    }
}
