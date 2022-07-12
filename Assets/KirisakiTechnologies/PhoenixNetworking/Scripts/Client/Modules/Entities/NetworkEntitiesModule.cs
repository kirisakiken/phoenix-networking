using System;
using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules.Entities
{
    public class NetworkEntitiesModule : GameModuleBaseMono, INetworkEntitiesModule
    {
        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _NetworkEventLogicModule = gameSystem.GetModule<INetworkEventLogicModule>();
            _NetworkEventLogicModule.OnUdpServerTickReceived += UdpServerTickReceivedHandler;

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Event Handlers

        private void UdpServerTickReceivedHandler(UdpServerTickPayload payload)
        {
            foreach (var genericNetworkEntity in payload.AddedEntities)
            {
                switch (genericNetworkEntity.EntityType)
                {
                    case EntityType.PlayerEntity:
                    {
                        // create player entity
                        Debug.Log("created entity");
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(genericNetworkEntity.EntityType));
                }
            }

            foreach (var genericNetworkEntity in payload.ModifiedEntities)
            {
                switch (genericNetworkEntity.EntityType)
                {
                    case EntityType.PlayerEntity:
                    {
                        // update player entity
                        Debug.Log("updated entity");
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(genericNetworkEntity.EntityType));
                }
            }

            foreach (var genericNetworkEntity in payload.RemovedEntities)
            {
                switch (genericNetworkEntity.EntityType)
                {
                    case EntityType.PlayerEntity:
                    {
                        // remove player entity
                        Debug.Log("removed entity");
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(genericNetworkEntity.EntityType));
                }
            }
        }

        #endregion

        #region Private

        private INetworkEventLogicModule _NetworkEventLogicModule;

        #endregion
    }
}
