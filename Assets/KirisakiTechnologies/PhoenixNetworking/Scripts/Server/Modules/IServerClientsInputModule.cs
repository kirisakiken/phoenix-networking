using System.Collections.Generic;

using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules
{
    /// <summary>
    ///     Represents a module that holds information related to
    ///     network client inputs
    /// </summary>
    public interface IServerClientsInputModule : IGameModule
    {
        /// <summary>
        ///     Clients and Inputs collection
        ///     Map(ClientId, ClientInputs)
        /// </summary>
        IReadOnlyDictionary<int, Dictionary<ClientInputKey, bool>> ClientInputs { get; }
    }
}