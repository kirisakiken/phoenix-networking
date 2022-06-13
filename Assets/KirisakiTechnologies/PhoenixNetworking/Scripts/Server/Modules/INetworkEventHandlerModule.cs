using KirisakiTechnologies.GameSystem.Scripts.Modules;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules
{
    /// <summary>
    ///     Holds description of various network events
    /// </summary>
    public interface INetworkEventHandlerModule : IGameModule
    {
        /// <summary>
        ///     Represents a network event when client connected
        /// </summary>
        void ClientConnected(int clientId, Packet packet);

        // /// <summary>
        // ///     Represents a network event for received client tick (ideally, key inputs)
        // /// </summary>
        // void ClientTick(int clientId, Packet packet);
    }
}