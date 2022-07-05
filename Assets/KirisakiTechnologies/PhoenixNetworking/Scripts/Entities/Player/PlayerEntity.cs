using JetBrains.Annotations;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Entities.Player
{
    public class PlayerEntity : IPlayerEntity
    {
        #region Constructors

        public PlayerEntity(int id, int clientId, [CanBeNull] string networkId)
        {
            Id = id;
            ClientId = clientId;
            NetworkId = string.IsNullOrEmpty(networkId)
                ? System.Guid.NewGuid().ToString()
                : networkId;
        }

        #endregion

        #region IPlayerEntity Implementation

        public int ClientId { get; }

        #endregion

        #region INetworkEntity Implementation

        public string NetworkId { get; }

        #endregion

        #region IEntity Implementation

        public int Id { get; }

        #endregion
    }
}