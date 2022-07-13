using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Entities;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.GameSystem.Scripts.Modules.Entities;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Entities.Player;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules.Entities
{
    public class PlayerEntitiesModule : GameModuleBaseMono, IPlayerEntitiesModule
    {
        #region IPlayerEntitiesModule Implementation

        // TODO: decide if this is necessary. Remove if not. (e.g. other sub modules can get player entity by id from IEntitiesModule)
        public IReadOnlyDictionary<int, IPlayerEntity> PlayerEntities => _PlayerEntities;

        #endregion

        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            EntitiesModule = gameSystem.GetModule<IEntitiesModule>();
            EntitiesModule.OnEntitiesChanged += EntitiesChangedHandler;

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Event Handlers

        private void EntitiesChangedHandler(ReadonlyEntitiesTransaction transaction)
        {
            foreach (var entity in transaction.AddedEntities)
            {
                if (!(entity is IPlayerEntity playerEntity))
                    continue;

                if (_PlayerEntities.ContainsKey(playerEntity.Id))
                    throw new Exception($"PlayerEntity[{playerEntity.Id}] already exists in collection {nameof(_PlayerEntities)}");

                _PlayerEntities.Add(playerEntity.Id, playerEntity);
            }

            foreach (var entity in transaction.ModifiedEntities)
            {
                if (!(entity is IPlayerEntity playerEntity))
                    continue;

                if (!_PlayerEntities.ContainsKey(playerEntity.Id))
                    throw new KeyNotFoundException($"Unable to find PlayerEntity[{playerEntity.Id}] in collection {nameof(_PlayerEntities)}");

                _PlayerEntities[playerEntity.Id] = playerEntity;
            }

            foreach (var entity in transaction.RemovedEntities)
            {
                if (!(entity is IPlayerEntity playerEntity))
                    continue;

                if (!_PlayerEntities.ContainsKey(playerEntity.Id))
                    throw new KeyNotFoundException($"Unable to find PlayerEntity[{playerEntity.Id}] in collection {nameof(_PlayerEntities)}");

                _PlayerEntities.Remove(playerEntity.Id);
            }
        }

        #endregion

        #region Private

        private IEntitiesModule EntitiesModule;

        private readonly Dictionary<int, IPlayerEntity> _PlayerEntities = new Dictionary<int, IPlayerEntity>();

        #endregion
    }
}