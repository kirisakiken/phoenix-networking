using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Entities;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Factories;
using KirisakiTechnologies.GameSystem.Scripts.Modules.Entities;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Entities.Player;

using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Factories.Entities
{
    public class PlayerEntitiesViewFactory : GameViewFactoryBaseMono, IPlayerEntitiesViewFactory
    {
        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            if (_PlayerEntityViewPrefab == null)
                throw new NullReferenceException(nameof(_PlayerEntityViewPrefab));

            _EntitiesModule = gameSystem.GetModule<IEntitiesModule>();
            _EntitiesModule.OnEntitiesChanged += EntitiesChangedHandler;

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Event Handler

        private void EntitiesChangedHandler(ReadonlyEntitiesTransaction transaction)
        {
            foreach (var entity in transaction.AddedEntities)
            {
                if (!(entity is IPlayerEntity playerEntity))
                    continue;

                CreateView(playerEntity);
            }

            foreach (var entity in transaction.ModifiedEntities)
            {
                if (!(entity is IPlayerEntity playerEntity))
                    continue;

                RedrawView(playerEntity);
            }

            foreach (var entity in transaction.RemovedEntities)
            {
                if (!(entity is IPlayerEntity playerEntity))
                    continue;

                RemoveView(playerEntity);
            }
        }

        #endregion

        #region Private

        [SerializeField]
        private GameObject _PlayerEntityViewPrefab;

        // TODO: change type from <IPlayerEntity, Transform> to <IPlayerEntity, IPlayerEntityView> (implement IPlayerEntityView)
        private readonly Dictionary<IPlayerEntity, GameObject> _PlayerEntitiesViews = new Dictionary<IPlayerEntity, GameObject>();

        private IEntitiesModule _EntitiesModule;

        private void CreateView(IPlayerEntity playerEntity)
        {
            if (_PlayerEntitiesViews.ContainsKey(playerEntity))
                throw new Exception($"View of PlayerEntity[{playerEntity.Id}] already exists in collection: {nameof(_PlayerEntitiesViews)}");

            var view = Instantiate(_PlayerEntityViewPrefab, playerEntity.Position, playerEntity.Rotation);
            _PlayerEntitiesViews.Add(playerEntity, view);
        }

        private void RedrawView(IPlayerEntity playerEntity)
        {
            if (!_PlayerEntitiesViews.ContainsKey(playerEntity))
                throw new KeyNotFoundException($"Unable to find view of PlayerEntity[{playerEntity.Id}] in collection: {nameof(_PlayerEntitiesViews)}");

            var view = _PlayerEntitiesViews[playerEntity];
            view.transform.position = playerEntity.Position;
            view.transform.rotation = playerEntity.Rotation;
        }

        private void RemoveView(IPlayerEntity playerEntity)
        {
            if (!_PlayerEntitiesViews.ContainsKey(playerEntity))
                throw new KeyNotFoundException($"Unable to find view of PlayerEntity[{playerEntity.Id}] in collection: {nameof(_PlayerEntitiesViews)}");

            _PlayerEntitiesViews.Remove(playerEntity);
        }

        #endregion
    }
}