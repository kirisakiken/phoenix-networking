using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Providers;
using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules
{
    public class NetworkEventHandlerModule : GameModuleBaseMono, INetworkEventHandlerModule
    {
        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _ServerModule = gameSystem.GetModule<IServerModule>();
            _ServerModule.OnClientConnected += ClientConnectedHandler;
            _ServerModule.OnClientConnectionHandshakeCompleted += ClientConnectionHandshakeCompletedHandler;
            // _ServerModule.OnClientDisconnected += ClientDisconnectedHandler;
            // _ServerModule.OnClientPayloadReceived += ClientPayloadReceivedHandler;

            _TcpPacketProvider = gameSystem.GetProvider<ITcpPacketProvider>();

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Event Handlers

        private void ClientConnectedHandler(int clientId)
        {
            // TODO: execute initial connect logic (e.g. (creating player in method ClientConnectionHandshakeCompletedHandler seems better) create player prefab, set clientId etc. and send that as packet to player
            // building payload . . . TODO: IMPORTANT move to TcpPacketProvider
            var payload = new TcpInitialConnectPayload
            {
                ClientId = clientId,
                AvailableClients = new List<ClientData>(),
            };

            foreach (var kvp in _ServerModule.Clients)
            {
                if (kvp.Value.ServerTcp.Id == clientId)
                    continue;

                if (!kvp.Value.ServerTcp.IsConnected)
                    continue;

                payload.AvailableClients.Add(new ClientData { ClientId = kvp.Value.Id, ClientName = kvp.Value.Name });
            }

            using (var packet = _TcpPacketProvider.ClientInitialConnectionPacket(payload))
                SendTcpData(clientId, packet);
        }

        private void ClientConnectionHandshakeCompletedHandler(int clientId, Packet packet)
        {
            var receivedId = packet.ReadInt();
            var receivedName = packet.ReadString();

            // TODO: not a good implementation. Refactor if possible
            if (_ServerModule.Clients.ContainsKey(receivedId))
                _ServerModule.Clients[receivedId].Name = receivedName;
            
            // TODO: add logic below to execute on client connected logic
            // e.g. Invoke event where ClientGenerationModule subs and creates player prefabs properly
            Debug.Log($"Network Handler: Message from client to server, ClientId: {receivedId} | Name: {receivedName}");

            // TODO: return early from condition below and force client out of server.
            if (clientId != receivedId)
                Debug.LogError($"Client Id and received id does not match!!! | ClientId: {clientId} , ReceivedId: {receivedId}");

            // building payload . . . TODO: IMPORTANT move to TcpPacketProvider or somewhere else
            var payload = new TcpConnectedClientBroadcastPayload
            {
                ClientData = new ClientData
                {
                    ClientId = receivedId,
                    ClientName = receivedName,
                },
            };

            using (var broadcastPacket = _TcpPacketProvider.ClientConnectReceivedBroadcastPacket(payload))
                SendTcpDataToAllExceptOne(clientId, broadcastPacket);

            // TODO: Handshake event and sub to it from NetworkEventLogicModule to implement logic needs to be executed
        }

        private void ClientDisconnectedHandler(int clientId, Packet packet)
        {
            throw new NotImplementedException($"{nameof(ClientDisconnectedHandler)} not implemented!");
        }

        private void ClientPayloadReceivedHandler(int clientId, Packet packet)
        {
            throw new NotImplementedException($"{nameof(ClientPayloadReceivedHandler)} not implemented!");
        }

        #endregion

        #region Private

        private IServerModule _ServerModule;
        private ITcpPacketProvider _TcpPacketProvider;

        private void SendTcpData(int clientId, Packet packet)
        {
            packet.WriteLength();

            if (!_ServerModule.Clients.ContainsKey(clientId))
                throw new InvalidOperationException($"Unable to find client with id: {clientId} in collection {nameof(_ServerModule.Clients)}");

            _ServerModule.Clients[clientId].ServerTcp.SendData(packet);
        }

        private void SendTcpDataToAll(Packet packet)
        {
            packet.WriteLength();

            foreach (var client in _ServerModule.Clients.Values)
                client.ServerTcp.SendData(packet);
        }

        private void SendTcpDataToAllExceptOne(int clientId, Packet packet)
        {
            packet.WriteLength();

            foreach (var client in _ServerModule.Clients.Values)
            {
                if (!client.ServerTcp.IsConnected)
                    continue;

                if (client.Id == clientId)
                    continue;

                client.ServerTcp.SendData(packet);
            }
        }

        #endregion
    }
}