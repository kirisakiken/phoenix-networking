using System.Threading.Tasks;
using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules
{
    // TODO: remember to add this under game system prefab client side
    public class NetworkLogicModule : GameModuleBaseMono, INetworkEventLogicModule
    {
        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _NetworkEventHandlerModule = gameSystem.GetModule<INetworkEventHandlerModule>();
            _NetworkEventHandlerModule.OnInitialConnectPackageReceived += InitialConnectPackageReceivedHandler;
            _NetworkEventHandlerModule.OnClientConnectedBroadcastPackageReceived += ClientConnectBroadcastReceivedHandler;

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Event Handlers

        private void InitialConnectPackageReceivedHandler(TcpInitialConnectPayload payload)
        {
            // TODO: implement
            Debug.LogWarning($"Not implemented Tcp Event Logic method handler. Implement {nameof(InitialConnectPackageReceivedHandler)}");

            var tx = GameObject.Find("OnConnectText").GetComponent<Text>();
            if (tx != null)
                tx.text = JsonConvert.SerializeObject(payload);
        }

        private void ClientConnectBroadcastReceivedHandler(TcpConnectedClientBroadcastPayload payload)
        {
            // TODO: implement
            Debug.LogWarning($"Not implemented Tcp Event Logic method handler. Implement {nameof(ClientConnectBroadcastReceivedHandler)}");

            var tx = GameObject.Find("OnConnectBroadcastText").GetComponent<Text>();
            if (tx != null)
                tx.text = JsonConvert.SerializeObject(payload);
        }

        #endregion

        #region Private

        private INetworkEventHandlerModule _NetworkEventHandlerModule;

        #endregion
    }
}