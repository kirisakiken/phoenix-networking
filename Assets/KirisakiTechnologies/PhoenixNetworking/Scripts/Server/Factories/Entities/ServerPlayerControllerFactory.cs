using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            foreach (var entity in transaction.ModifiedEntities)
            {
                if (!(entity is IPlayerEntity playerEntity))
                    continue;

                // update player
                throw new NotImplementedException();
            }

            foreach (var entity in transaction.RemovedEntities)
            {
                if (!(entity is IPlayerEntity playerEntity))
                    continue;

                // remove player model
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Private

        [SerializeField]
        private ServerPlayerController _ServerPlayerControllerPrefab;

        private readonly Dictionary<IEntity, IServerPlayerController> _Controllers = new Dictionary<IEntity, IServerPlayerController>();

        private IGameSystem _GameSystem;
        private IEntitiesModule _EntitiesModule;

        private void CreateController(IPlayerEntity playerEntity)
        {
            if (playerEntity == null)
                throw new ArgumentNullException(nameof(playerEntity));

            if (_ServerPlayerControllerPrefab == null)
                throw new NullReferenceException(nameof(_ServerPlayerControllerPrefab));

            if (_Controllers.ContainsKey(playerEntity))
                throw new Exception($"Player entity: {playerEntity.Id} already exists in the collection: {nameof(_Controllers)}");

            var controller = Instantiate(_ServerPlayerControllerPrefab);
            controller.PlayerEntity = playerEntity;
            controller.Initialize(_GameSystem);

            _Controllers.Add(playerEntity, controller);
        }

        #endregion
    }
}
