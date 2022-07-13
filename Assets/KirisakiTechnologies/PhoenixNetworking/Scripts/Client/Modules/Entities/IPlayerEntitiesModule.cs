using System.Collections.Generic;

using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Entities.Player;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules.Entities
{
    // TODO: add description
    public interface IPlayerEntitiesModule : IGameModule
    {
        IReadOnlyDictionary<int, IPlayerEntity> PlayerEntities { get; }
    }
}