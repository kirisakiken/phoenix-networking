using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules
{
    public class NetworkEventLogicModule : GameModuleBaseMono, INetworkEventLogicModule
    {
        #region INetworkEventLogicModule Implementation

        public event NetworkLogicEvent<UdpClientInputPayload> OnUdpClientInputPayloadReceived;
        public event NetworkLogicEvent<TcpConnectedClientBroadcastPayload> OnClientConnectionHandshakeCompleted;

        #endregion

        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _NetworkEventHandlerModule = gameSystem.GetModule<INetworkEventHandlerModule>();
            _NetworkEventHandlerModule.OnUdpClientInputPayloadReceived += UdpClientInputPayloadReceivedHandler;
            _NetworkEventHandlerModule.OnClientConnectionHandshakeCompleted += ClientConnectionHandshakeCompleted;

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Event Handlers

        private void UdpClientInputPayloadReceivedHandler(int clientId, UdpClientInputPayload payload)
        {
            // TODO: remove debugging
            Debug.Log($"Received udp client input. FROM: Client[{clientId}]");
            foreach (var kvp in payload.ModifiedKeys)
            {
                Debug.Log($"Client[{clientId}]:{kvp.Key}|{kvp.Value}");
            }

            OnUdpClientInputPayloadReceived?.Invoke(clientId, payload); // TODO: sub to this from e.g. IServerClientControllerModule
        }

        private void ClientConnectionHandshakeCompleted(int clientId, TcpConnectedClientBroadcastPayload payload) => OnClientConnectionHandshakeCompleted?.Invoke(clientId, payload);

        #endregion

        #region Private

        private INetworkEventHandlerModule _NetworkEventHandlerModule;

        #endregion
    }
}