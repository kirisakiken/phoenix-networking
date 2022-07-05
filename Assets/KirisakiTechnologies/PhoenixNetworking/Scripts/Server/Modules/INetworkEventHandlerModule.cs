using KirisakiTechnologies.GameSystem.Scripts.Modules;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules
{
    /// <summary>
    ///     Holds ownership of various network events
    /// </summary>
    public interface INetworkEventHandlerModule : IGameModule
    {
        void SendUdpData(int clientId, Packet packet);

        void SendUdpDataToAll(Packet packet);

        void SendUdpDataToAllExceptOne(int clientId, Packet packet);
    }
}