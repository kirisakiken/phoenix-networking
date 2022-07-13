using JetBrains.Annotations;
using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Entities.Player
{
    public class PlayerEntity : IPlayerEntity
    {
        #region Constructors

        public PlayerEntity(int id, int clientId, string clientName, [CanBeNull] string networkId)
        {
            Id = id;
            ClientId = clientId;
            ClientName = clientName;
            NetworkId = string.IsNullOrEmpty(networkId)
                ? System.Guid.NewGuid().ToString()
                : networkId;
        }

        #endregion

        #region IPlayerEntity Implementation

        public int ClientId { get; }
        public string ClientName { get; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        #endregion

        #region INetworkEntity Implementation

        public string NetworkId { get; }

        #endregion

        #region IEntity Implementation

        public int Id { get; }

        #endregion
    }
}