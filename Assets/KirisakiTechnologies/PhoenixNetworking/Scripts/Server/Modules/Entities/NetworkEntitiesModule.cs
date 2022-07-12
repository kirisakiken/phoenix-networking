using System;
using System.Collections.Generic;

using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
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

        #region Private

        private readonly Dictionary<INetworkEntity, NetworkEntityState> _NetworkEntities = new Dictionary<INetworkEntity, NetworkEntityState>();

        #endregion
    }
}