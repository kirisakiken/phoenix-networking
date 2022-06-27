using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules
{
    // TODO: remember to add this under game system prefab client side
    public class NetworkEventLogicModule : GameModuleBaseMono, INetworkEventLogicModule
    {
        #region INetworkEventLogicModule

        public event NetworkLogicEvent<TcpInitialConnectPayload> OnTcpInitialConnectPayloadReceived;

        public event NetworkLogicEvent<TcpConnectedClientBroadcastPayload> OnTcpConnectedClientBroadcastPayloadReceived;

        public event NetworkLogicEvent<TcpClientMessagePayload> OnTcpClientMessagePayloadReceived;

        #endregion

        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _NetworkEventHandlerModule = gameSystem.GetModule<INetworkEventHandlerModule>();
            _NetworkEventHandlerModule.OnInitialConnectPackageReceived += InitialConnectPackageReceivedHandler;
            _NetworkEventHandlerModule.OnClientConnectedBroadcastPackageReceived += ClientConnectBroadcastReceivedHandler;
            _NetworkEventHandlerModule.OnClientTcpMessagePayloadPackageReceived += ClientTcpMessagePayloadPackageReceived;

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Event Handlers

        private void InitialConnectPackageReceivedHandler(TcpInitialConnectPayload payload) => OnTcpInitialConnectPayloadReceived?.Invoke(payload);

        private void ClientConnectBroadcastReceivedHandler(TcpConnectedClientBroadcastPayload payload) => OnTcpConnectedClientBroadcastPayloadReceived?.Invoke(payload);

        private void ClientTcpMessagePayloadPackageReceived(TcpClientMessagePayload payload) => OnTcpClientMessagePayloadReceived?.Invoke(payload);

        #endregion

        #region Private

        private INetworkEventHandlerModule _NetworkEventHandlerModule;

        #endregion
    }
}