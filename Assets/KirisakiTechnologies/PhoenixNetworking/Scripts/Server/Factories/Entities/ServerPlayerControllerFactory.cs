using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using JetBrains.Annotations;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Entities;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Factories;
using KirisakiTechnologies.GameSystem.Scripts.Modules.Entities;
using KirisakiTechnologies.PhoenixNetworking.PlayerController.Scripts;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Entities.Player;

using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Factories.Entities
{
    public class ServerPlayerControllerFactory : GameControllerFactoryBaseMono, IServerPlayerControllerFactory
    {
        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _GameSystem = gameSystem;
            _EntitiesModule = gameSystem.GetModule<IEntitiesModule>();
            _EntitiesModule.OnEntitiesChanged += EntitiesChangedHandler;

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

                CreateController(playerEntity);
            }

            foreach (var entity in transaction.RemovedEntities)
            {
                if (!(entity is IPlayerEntity playerEntity))
                    continue;

                RemoveController(playerEntity);
            }
        }

        #endregion

        #region Private

        [SerializeField]
        private ServerPlayerController _ServerPlayerControllerPrefab;

        private readonly Dictionary<int, IServerPlayerController> _Controllers = new Dictionary<int, IServerPlayerController>();

        private IGameSystem _GameSystem;
        private IEntitiesModule _EntitiesModule;

        private void CreateController([NotNull] IPlayerEntity playerEntity)
        {
            if (playerEntity == null)
                throw new ArgumentNullException(nameof(playerEntity));

            if (_ServerPlayerControllerPrefab == null)
                throw new NullReferenceException(nameof(_ServerPlayerControllerPrefab));

            if (_Controllers.ContainsKey(playerEntity.Id))
                throw new Exception($"Player entity: {playerEntity.Id} already exists in the collection: {nameof(_Controllers)}");

            var controller = Instantiate(_ServerPlayerControllerPrefab);
            controller.PlayerEntity = playerEntity;
            controller.Initialize(_GameSystem);

            _Controllers.Add(playerEntity.Id, controller);
        }

        private void RemoveController([NotNull] IPlayerEntity playerEntity)
        {
            if (playerEntity == null)
                throw new ArgumentNullException(nameof(playerEntity));

            if (!_Controllers.ContainsKey(playerEntity.Id))
                throw new KeyNotFoundException($"Unable to find PlayerEntity[{playerEntity.Id}] in collection: {nameof(_Controllers)}");

            _Controllers.Remove(playerEntity.Id);
        }

        #endregion
    }
}
