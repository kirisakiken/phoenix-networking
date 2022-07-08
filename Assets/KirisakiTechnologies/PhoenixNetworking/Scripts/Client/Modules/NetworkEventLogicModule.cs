using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Providers;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;
using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules
{
    public class NetworkEventLogicModule : GameModuleBaseMono, INetworkEventLogicModule
    {
        #region INetworkEventLogicModule

        public event NetworkLogicEvent<TcpInitialConnectPayload> OnTcpInitialConnectPayloadReceived;
        public event NetworkLogicEvent<TcpConnectedClientBroadcastPayload> OnTcpConnectedClientBroadcastPayloadReceived;
        public event NetworkLogicEvent<TcpClientMessagePayload> OnTcpClientMessagePayloadReceived;
        public event NetworkLogicEvent<UdpPayload> OnUdpPayloadReceived;
        public event NetworkLogicEvent<UdpServerTickPayload> OnUdpServerTickReceived; 
        public event NetworkLogicEvent<int> OnHandshakePacketRequested;

        #endregion

        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _NetworkEventHandlerModule = gameSystem.GetModule<INetworkEventHandlerModule>();
            _NetworkEventHandlerModule.OnInitialConnectPackageReceived += InitialConnectPackageReceivedHandler;
            _NetworkEventHandlerModule.OnClientConnectedBroadcastPackageReceived += ClientConnectBroadcastReceivedHandler;
            _NetworkEventHandlerModule.OnClientTcpMessagePayloadPackageReceived += ClientTcpMessagePayloadPackageReceived;
            _NetworkEventHandlerModule.OnUdpPayloadReceived += UdpPayloadReceivedHandler;
            _NetworkEventHandlerModule.OnUdpServerTickReceived += UdpServerTickReceivedHandler;
            _NetworkEventHandlerModule.OnHandshakePacketRequested += HandshakePacketRequestedHandler;

            _TcpPacketProvider = gameSystem.GetProvider<ITcpPacketProvider>();

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Event Handlers

        private void InitialConnectPackageReceivedHandler(TcpInitialConnectPayload payload) => OnTcpInitialConnectPayloadReceived?.Invoke(payload);

        private void ClientConnectBroadcastReceivedHandler(TcpConnectedClientBroadcastPayload payload) => OnTcpConnectedClientBroadcastPayloadReceived?.Invoke(payload);

        private void ClientTcpMessagePayloadPackageReceived(TcpClientMessagePayload payload) => OnTcpClientMessagePayloadReceived?.Invoke(payload);

        private void UdpPayloadReceivedHandler(UdpPayload payload)
        {
            Debug.Log($"Received udp message: {payload.Message}"); // TODO: remove after tests
            OnUdpPayloadReceived?.Invoke(payload);
        }

        private void UdpServerTickReceivedHandler(UdpServerTickPayload payload)
        {
            // TODO: remove
            Debug.Log("Received udp server tick. Contents;");
            payload.AddedEntities.ForEach(entity =>
            {
                Debug.Log($"Type: {entity.EntityType},\n{entity.Data}");
            });
            payload.ModifiedEntities.ForEach(entity =>
            {
                Debug.Log($"Type: {entity.EntityType},\n{entity.Data}");
            });
            payload.RemovedEntities.ForEach(entity =>
            {
                Debug.Log($"Type: {entity.EntityType},\n{entity.Data}");
            });

            OnUdpServerTickReceived?.Invoke(payload);
        }

        private void HandshakePacketRequestedHandler(int payload)
        {
            // TODO: IMPORTANT!!! Find proper way to send name to server
            using (var packet = _TcpPacketProvider.OnConnectWelcomeReceivedPacket(payload, "RandomName"))
                _NetworkEventHandlerModule.SendTcpDataToServer(packet);

            OnHandshakePacketRequested?.Invoke(payload);
        }

        #endregion

        #region Private

        private INetworkEventHandlerModule _NetworkEventHandlerModule;
        private ITcpPacketProvider _TcpPacketProvider;

        #endregion
    }
}