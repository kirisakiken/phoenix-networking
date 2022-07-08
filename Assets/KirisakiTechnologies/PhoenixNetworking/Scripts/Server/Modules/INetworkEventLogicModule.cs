using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules
{
    // TODO: add description
    public delegate void NetworkLogicEvent<in T>(int clientId, T payload);

    // TODO: add description
    public interface INetworkEventLogicModule : IGameModule
    {
        event NetworkLogicEvent<UdpClientInputPayload> OnUdpClientInputPayloadReceived;
    }
}