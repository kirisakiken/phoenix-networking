using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Entities;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.GameSystem.Scripts.Modules.Entities;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Entities;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules.Entities
{
    public class NetworkEntitiesModule : GameModuleBaseMono, INetworkEntitiesModule
    {
        #region INetworkEntitiesModule Implementation

        public IReadOnlyDictionary<INetworkEntity, NetworkEntityState> NetworkEntities => _NetworkEntities;

        public void AddDirty(INetworkEntity networkEntity, NetworkEntityState state)
        {
            if (networkEntity == null)
                throw new ArgumentNullException(nameof(networkEntity));

            // switch (networkEntity)
            // {
            //     case IPlayerEntity playerEntity:
            //     {
            //         _NetworkEntities.Add(playerEntity, state);
            //         break;
            //     }
            //     default:
            //         throw new ArgumentOutOfRangeException(nameof(networkEntity));
            // }

            if (_NetworkEntities.ContainsKey(networkEntity))
            {
                _NetworkEntities[networkEntity] = state;
                return;
            }

            _NetworkEntities.Add(networkEntity, state);
        }

        public void CleanStates()
        {
            _NetworkEntities.UpdateAll(NetworkEntityState.Unchanged);
        }

        #endregion

        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _EntitiesModule = gameSystem.GetModule<IEntitiesModule>();
            _EntitiesModule.OnEntitiesChanged += EntitiesChangedHandler;

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Event Handlers

        private void EntitiesChangedHandler(ReadonlyEntitiesTransaction payload)
        {
            foreach (var entity in payload.AddedEntities)
            {
                if (!(entity is INetworkEntity networkEntity))
                    continue;

                AddDirty(networkEntity, NetworkEntityState.Added);
            }

            foreach (var entity in payload.ModifiedEntities)
            {
                if (!(entity is INetworkEntity networkEntity))
                    continue;

                AddDirty(networkEntity, NetworkEntityState.Modified);
            }

            foreach (var entity in payload.RemovedEntities)
            {
                if (!(entity is INetworkEntity networkEntity))
                    continue;

                AddDirty(networkEntity, NetworkEntityState.Removed);
            }
        }

        #endregion

        #region Private

        private readonly Dictionary<INetworkEntity, NetworkEntityState> _NetworkEntities = new Dictionary<INetworkEntity, NetworkEntityState>();

        private IEntitiesModule _EntitiesModule;

        #endregion
    }
}