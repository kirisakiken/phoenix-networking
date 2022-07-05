using KirisakiTechnologies.GameSystem.Scripts.Entities;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Entities
{
    // TODO: add description
    public interface INetworkEntity : IEntity
    {
        // uuid network id for network-able entities
        string NetworkId { get; }
    }
}