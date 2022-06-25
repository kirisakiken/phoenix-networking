using KirisakiTechnologies.GameSystem.Scripts.Providers;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Providers
{
    /// <summary>
    ///     Packet builder/provider for various network events
    /// </summary>
    public interface ITcpPacketProvider : IGameProvider
    {
        /// <summary>
        ///     Received message from server on initial connection
        /// </summary>
        string DeserializeOnClientConnectedPacket(Packet packet, out int receivedId);

        /// <summary>
        ///     Received broadcast message from server when another client connects
        /// </summary>
        string DeserializeOnClientConnectedBroadcastReceivedPacket(Packet packet, out int receivedClientId);

        /// <summary>
        ///     Packet needs to be build to send welcome received information packet
        /// </summary>
        Packet OnConnectWelcomeReceivedPacket(int clientId, string message);
    }
}