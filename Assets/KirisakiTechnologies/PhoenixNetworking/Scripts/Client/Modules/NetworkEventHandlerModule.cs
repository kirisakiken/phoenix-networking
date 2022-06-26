using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Providers;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules
{
    public class NetworkEventHandlerModule : GameModuleBaseMono, INetworkEventHandlerModule
    {
        #region INetworkEventHandlerModule Implementation

        public event TcpReceiveEvent<TcpInitialConnectPayload> OnInitialConnectPackageReceived;
        public event TcpReceiveEvent<TcpConnectedClientBroadcastPayload> OnClientConnectedBroadcastPackageReceived; 

        #endregion

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
            var receivedData = _TcpPacketProvider.DeserializeOnClientInitialConnectionPacket(receivedPacket);
            Debug.Log($"ClientNetworkModule, received on connect data: {receivedData}");

            var welcomeReceivedPacket = _TcpPacketProvider.OnConnectWelcomeReceivedPacket(receivedData.ClientId, $"AliBaba");
            SendTcpDataToServer(welcomeReceivedPacket);

            OnInitialConnectPackageReceived?.Invoke(receivedData);
        }

        private void ClientConnectedBroadcastReceivedHandler(Packet receivedPacket)
        {
            // TODO: execute connected client broadcast logic (e.g. create connected player prefab)
            var receivedData = _TcpPacketProvider.DeserializeOnClientConnectedBroadcastReceivedPacket(receivedPacket);
            Debug.Log($"ClientNetworkModule, received on connect connected broadcast data: {receivedData}");

            OnClientConnectedBroadcastPackageReceived?.Invoke(receivedData);
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