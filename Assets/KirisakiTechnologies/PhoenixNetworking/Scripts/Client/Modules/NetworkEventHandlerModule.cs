using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Providers;
using UnityEngine;
using UnityEngine.UI;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules
{
    public class NetworkEventHandlerModule : GameModuleBaseMono, INetworkEventHandlerModule
    {
        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _ClientModule = gameSystem.GetModule<IClientModule>();
            _ClientModule.OnClientConnected += ClientConnectedHandler;
            _ClientModule.OnClientConnectBroadcastReceived += ClientConnectedBroadcastReceivedHandler;

            _TcpPacketProvider = gameSystem.GetProvider<ITcpPacketProvider>();

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Event Handlers

        private void ClientConnectedHandler(Packet receivedPacket)
        {
            // TODO: execute initial connect logic (e.g. create player prefab (maybe invoke events and execute network logic with another module e.g. NetworkLogicModule)
            var receivedData = _TcpPacketProvider.DeserializeOnClientConnectedPacket(receivedPacket, out var receivedId);
            Debug.Log($"ClientNetworkModule, received on connect data: {receivedData}");

            var welcomeReceivedPacket = _TcpPacketProvider.OnConnectWelcomeReceivedPacket(receivedId, $"AliBaba");
            SendTcpDataToServer(welcomeReceivedPacket);
        }

        private void ClientConnectedBroadcastReceivedHandler(Packet receivedPacket)
        {
            // TODO: execute connected client broadcast logic (e.g. create connected player prefab)
            var receivedData = _TcpPacketProvider.DeserializeOnClientConnectedBroadcastReceivedPacket(receivedPacket, out var receivedClientId);
            Debug.Log($"ClientNetworkModule, received on connect connected broadcast data: {receivedData}");
        }

        #endregion

        #region Private

        private IClientModule _ClientModule;
        private ITcpPacketProvider _TcpPacketProvider;

        private void SendTcpDataToServer(Packet packet)
        {
            packet.WriteLength();

            _ClientModule.Tcp.SendData(packet);
        }

        #endregion
    }
}