using System;
using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Entities;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.GameSystem.Scripts.Modules.Entities;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Entities.Player;

using Newtonsoft.Json;

using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules.Entities
{
    public class NetworkEntitiesModule : GameModuleBaseMono, INetworkEntitiesModule
    {
        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _EntitiesModule = gameSystem.GetModule<IEntitiesModule>();

            _NetworkEventLogicModule = gameSystem.GetModule<INetworkEventLogicModule>();
            _NetworkEventLogicModule.OnUdpServerTickReceived += UdpServerTickReceivedHandler;

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Event Handlers

        private void UdpServerTickReceivedHandler(UdpServerTickPayload payload)
        {
            var transaction = new EntitiesTransaction();
            foreach (var genericNetworkEntity in payload.AddedEntities)
            {
                switch (genericNetworkEntity.EntityType)
                {
                    case EntityType.PlayerEntity:
                    {
                        // create player entity
                        Debug.Log("added entity");

                        // TODO: deserializing can be moved somewhere else maybe?
                        var playerNetworkEntity = JsonConvert.DeserializeObject<PlayerNetworkEntity>((string) genericNetworkEntity.Data);
                        if (playerNetworkEntity == null)
                            throw new Exception("Unable to deserialize added entity data from payload");

                        var playerEntity = new PlayerEntity(playerNetworkEntity.EntityId, playerNetworkEntity.ClientId, playerNetworkEntity.ClientName, playerNetworkEntity.NetworkId)
                        {
                            Position = new Vector3(playerNetworkEntity.Position.X, playerNetworkEntity.Position.Y, playerNetworkEntity.Position.Z),
                            Rotation = new Quaternion(playerNetworkEntity.Rotation.X, playerNetworkEntity.Rotation.Y, playerNetworkEntity.Rotation.Z, playerNetworkEntity.Rotation.W),
                        };

                        transaction.AddedEntities.Add(playerEntity);
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

                        var playerNetworkEntity = JsonConvert.DeserializeObject<PlayerNetworkEntity>((string) genericNetworkEntity.Data);
                        if (playerNetworkEntity == null)
                            throw new Exception("Unable to deserialize modified entity data from payload");

                        var playerEntity = _EntitiesModule.GetEntity<IPlayerEntity>(playerNetworkEntity.EntityId);
                        if (playerEntity == null)
                            throw new Exception($"Unable to find player entity with id: {playerNetworkEntity.EntityId}");

                        playerEntity.Position = new Vector3(playerNetworkEntity.Position.X, playerNetworkEntity.Position.Y, playerNetworkEntity.Position.Z);
                        playerEntity.Rotation = new Quaternion(playerNetworkEntity.Rotation.X, playerNetworkEntity.Rotation.Y, playerNetworkEntity.Rotation.Z, playerNetworkEntity.Rotation.W);

                        transaction.ModifiedEntities.Add(playerEntity);
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

                        var playerNetworkEntity = JsonConvert.DeserializeObject<PlayerNetworkEntity>((string) genericNetworkEntity.Data);
                        if (playerNetworkEntity == null)
                            throw new Exception("Unable to deserialize removed entity data from payload");

                        var playerEntity = _EntitiesModule.GetEntity<IPlayerEntity>(playerNetworkEntity.EntityId); // TODO: IMPORTANT!!! Change 1 with PlayerNetworkEntity.EntityId;
                        if (playerEntity == null)
                            throw new Exception("Unable to find player entity with id: 1");

                        transaction.RemovedEntities.Add(playerEntity);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(genericNetworkEntity.EntityType));
                }
            }

            // TODO: invoke event here and collect position/rotation of entities (which we didn't processed above) (but also, update entities invokes updated entities event?)
            _EntitiesModule.UpdateEntities(transaction);
        }

        #endregion

        #region Private

        private IEntitiesModule _EntitiesModule;
        private INetworkEventLogicModule _NetworkEventLogicModule;

        #endregion
    }
}
