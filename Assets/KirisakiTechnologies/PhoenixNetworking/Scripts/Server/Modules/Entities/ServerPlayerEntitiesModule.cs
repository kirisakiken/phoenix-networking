using System.Collections.Generic;
using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Entities;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.GameSystem.Scripts.Modules.Entities;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Entities.Player;
using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules.Entities
{
    public class ServerPlayerEntitiesModule : GameModuleBaseMono, IServerPlayerEntitiesModule
    {
        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _EntitiesModule = gameSystem.GetModule<IEntitiesModule>();
            _NetworkEventLogicModule = gameSystem.GetModule<INetworkEventLogicModule>();
            _NetworkEventLogicModule.OnClientConnectionHandshakeCompleted += ClientConnectionHandshakeCompletedHandler;

            return Task.CompletedTask;
        }

        #endregion

        #region Event Handlers

        private void ClientConnectionHandshakeCompletedHandler(int clientId, TcpConnectedClientBroadcastPayload payload)
        {
            var entity = CreatePlayerEntity(clientId, payload);
            var transaction = new EntitiesTransaction();
            transaction.AddedEntities.Add(entity);

            _EntitiesModule.UpdateEntities(transaction);
        }

        #endregion

        #region Private

        private IEntitiesModule _EntitiesModule;
        private INetworkEventLogicModule _NetworkEventLogicModule;

        private IPlayerEntity CreatePlayerEntity(int clientId, TcpConnectedClientBroadcastPayload payload)
        {
            var id = _EntitiesModule.GetNextEntityId();
            return new PlayerEntity(id, clientId, payload.ClientData.ClientName, null);
        }

        #endregion
    }
}