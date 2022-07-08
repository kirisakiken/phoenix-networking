using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules
{
    // TODO: add description
    public delegate void NetworkReceiveEvent<in T>(int clientId, T payload);

    /// <summary>
    ///     Holds ownership of various network events
    /// </summary>
    public interface INetworkEventHandlerModule : IGameModule
    {
        event NetworkReceiveEvent<UdpClientInputPayload> OnUdpClientInputPayloadReceived;

        void SendUdpData(int clientId, Packet packet);

        void SendUdpDataToAll(Packet packet);

        void SendUdpDataToAllExceptOne(int clientId, Packet packet);
    }
}