using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules
{
    public class ServerClientsInputModule : GameModuleBaseMono, IServerClientsInputModule
    {
        #region IServerClientsInputModule

        public IReadOnlyDictionary<int, Dictionary<ClientInputKey, bool>> ClientInputs => _ClientInputs;

        #endregion

        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _NetworkEventLogicModule = gameSystem.GetModule<INetworkEventLogicModule>();
            _NetworkEventLogicModule.OnUdpClientInputPayloadReceived += UdpClientInputPayloadReceivedHandler;
            // _NetworkEventLogicModule.OnClientDisconnected += ClientDisconnectedHandler; // TODO: remove _ClientInputs[clientId] when client disconnected!

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Private

        private readonly Dictionary<int, Dictionary<ClientInputKey, bool>> _ClientInputs = new Dictionary<int, Dictionary<ClientInputKey, bool>>(); // TODO: find a way to make the value IReadOnlyDict

        private INetworkEventLogicModule _NetworkEventLogicModule;

        private void UdpClientInputPayloadReceivedHandler(int clientId, UdpClientInputPayload payload)
        {
            if (!_ClientInputs.ContainsKey(clientId))
            {
                var dict = new Dictionary<ClientInputKey, bool>();
                foreach (ClientInputKey clientInputKey in Enum.GetValues(typeof(ClientInputKey)))
                    dict.Add(clientInputKey, false);

                _ClientInputs.Add(clientId, dict);
            }

            foreach (var modifiedInput in payload.ModifiedKeys)
                _ClientInputs[clientId][modifiedInput.Key] = modifiedInput.Value;
        }

        #endregion
    }
}