using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Providers;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules
{
    public class NetworkEventHandlerModule : GameModuleBaseMono, INetworkEventHandlerModule
    {
        #region INetworkEventHandlerModule Implementation

        public event TcpReceiveEvent<TcpInitialConnectPayload> OnInitialConnectPackageReceived;
        public event TcpReceiveEvent<TcpConnectedClientBroadcastPayload> OnClientConnectedBroadcastPackageReceived;
        public event TcpReceiveEvent<TcpClientMessagePayload> OnClientTcpMessagePayloadPackageReceived;
        public event TcpReceiveEvent<int> OnHandshakePacketRequested;

        public void SendTcpDataToServer(Packet packet)
        {
            packet.WriteLength();
            _ClientModule.Tcp.SendData(packet);
        }

        public void SendUdpDataToServer(Packet packet)
        {
            packet.WriteLength();
            _ClientModule.Udp.SendData(packet);
        }

        public void SendTcpClientMessageToServer(int clientId, string message)
        {
            using (var messagePacket = _TcpPacketProvider.TcpClientMessagePacket(_ClientModule.Id, message))
                SendTcpDataToServer(messagePacket);
        }

        #endregion

        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _ClientModule = gameSystem.GetModule<IClientModule>();
            _ClientModule.OnClientConnected += ClientConnectedHandler;
            _ClientModule.OnClientConnectBroadcastReceived += ClientConnectedBroadcastReceivedHandler;
            _ClientModule.OnClientTcpMessagePayloadReceived += ClientTcpMessagePayloadReceived;

            _TcpPacketProvider = gameSystem.GetProvider<ITcpPacketProvider>();

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Event Handlers

        private void ClientConnectedHandler(Packet receivedPacket)
        {
            var receivedData = _TcpPacketProvider.DeserializeOnClientInitialConnectionPacket(receivedPacket);
            _ClientModule.Id = receivedData.ClientId;

            // TODO: not a good place to send handshake here
            // using (var welcomeReceivedPacket = _TcpPacketProvider.OnConnectWelcomeReceivedPacket(receivedData.ClientId, $"AliBaba"))
            //     SendTcpDataToServer(welcomeReceivedPacket);
            OnHandshakePacketRequested?.Invoke(receivedData.ClientId);

            OnInitialConnectPackageReceived?.Invoke(receivedData);
        }

        private void ClientConnectedBroadcastReceivedHandler(Packet receivedPacket)
        {
            var receivedData = _TcpPacketProvider.DeserializeOnClientConnectedBroadcastReceivedPacket(receivedPacket);
            OnClientConnectedBroadcastPackageReceived?.Invoke(receivedData);
        }

        private void ClientTcpMessagePayloadReceived(Packet receivedPacket)
        {
            var receivedData = _TcpPacketProvider.DeserializeOnClientTcpMessagePayloadReceivedPacket(receivedPacket);
            OnClientTcpMessagePayloadPackageReceived?.Invoke(receivedData);
        }

        #endregion

        #region Private

        private IClientModule _ClientModule;
        private ITcpPacketProvider _TcpPacketProvider;

        #endregion
    }
}