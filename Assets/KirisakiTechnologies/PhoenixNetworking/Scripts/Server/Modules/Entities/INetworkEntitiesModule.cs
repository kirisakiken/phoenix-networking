using System.Collections.Generic;

using JetBrains.Annotations;

using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Entities;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules.Entities
{
    /// <summary>
    ///     TODO: add description
    ///     Module is fine overall. But looks bad since outsiders are allowed to modify collection belongs to this module
    /// </summary>
    public interface INetworkEntitiesModule : IGameModule
    {
        /// <summary>
        ///     Represents available network entities and states of those
        /// </summary>
        IReadOnlyDictionary<INetworkEntity, NetworkEntityState> NetworkEntities { get; }

        /// <summary>
        ///     Modifies state of a network entity
        /// </summary>
        void AddDirty([NotNull] INetworkEntity networkEntity, NetworkEntityState state);

        /// <summary>
        ///     Cleans state of network entities back to 'Unchanged'
        /// </summary>
        void CleanStates();
    }
}