using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Entities.Player
{
    /// <summary>
    ///     Represents a player entity
    /// </summary>
    public interface IPlayerEntity : INetworkEntity
    {
        /// <summary>
        ///     Id of the client
        /// </summary>
        int ClientId { get; }

        /// <summary>
        ///     Name of the client
        /// </summary>
        string ClientName { get; }

        /// <summary>
        ///     Root position of client view
        /// </summary>
        Vector3 Position { get; set; }

        /// <summary>
        ///     Root rotation of client view
        /// </summary>
        Quaternion Rotation { get; set; }
    }
}