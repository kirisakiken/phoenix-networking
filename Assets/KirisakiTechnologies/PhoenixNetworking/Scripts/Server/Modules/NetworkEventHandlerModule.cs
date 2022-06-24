using System;
using System.Threading.Tasks;
using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
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
            _ServerModule.ClientConnectedHandler += ClientConnectedHandler;
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
            using (var packet = _TcpPacketProvider.ClientConnectedPacket(clientId, $"Network Module: Initial connect executed. Sending information to client: {clientId}"))
                SendTcpData(clientId, packet);
        }

        private void ClientConnectionHandshakeCompletedHandler(int clientId, Packet packet)
        {
            var receivedId = packet.ReadInt();
            var username = packet.ReadString();
            
            // TODO: add logic below to execute on client connected logic
            // e.g. Invoke event where ClientGenerationModule subs and creates player prefabs properly
            Debug.Log($"Network Handler: Message from client to server, ClientId: {receivedId} | Name: {username}");

            // TODO: return early from condition below and force client out of server. after fixing bug
            if (clientId != receivedId)
                Debug.LogError($"Client Id and received id does not match!!! | ClientId: {clientId} , ReceivedId: {receivedId}");

            // broadcast connected client to others
            // TODO: handle sent data on client side
            using (var broadcastPacket = _TcpPacketProvider.ClientConnectReceivedBroadcastPacket(clientId, $"Network Event Handler Module: broadcasting connected client to all id: {clientId} username: {username}"))
                SendTcpDataToAllExceptOne(clientId, broadcastPacket);
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