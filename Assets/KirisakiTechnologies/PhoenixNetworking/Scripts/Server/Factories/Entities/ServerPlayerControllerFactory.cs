using System;
using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Entities;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Factories;
using KirisakiTechnologies.GameSystem.Scripts.Modules.Entities;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Entities.Player;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Factories.Entities
{
    public class ServerPlayerControllerFactory : GameControllerFactoryBaseMono, IServerPlayerControllerFactory
    {
        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
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

                // instantiate and draw player model
                CreateView(playerEntity);
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

        // [SerializeField]
        // private IServerPlayerController _ServerPlayerControllerPrefab;

        // private readonly Dictionary<IEntity, IServerPlayerController> _Controllers = new Dictionary<IEntity, IServerPlayerController>();

        private IEntitiesModule _EntitiesModule;

        private void CreateView(IPlayerEntity playerEntity)
        {
            
        }

        private void Redraw()
        {
            
        }

        #endregion
    }
}
