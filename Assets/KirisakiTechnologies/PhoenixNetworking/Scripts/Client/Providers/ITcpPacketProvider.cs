using KirisakiTechnologies.GameSystem.Scripts.Providers;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Providers
{
    /// <summary>
    ///     Packet builder/provider for various network events
    /// </summary>
    public interface ITcpPacketProvider : IGameProvider
    {
        string DeserializeOnClientConnectedPacket(Packet packet, out int receivedId);

        /// <summary>
        ///     Packet needs to be build to send welcome received information packet
        /// </summary>
        Packet OnConnectWelcomeReceivedPacket(int clientId, string message);
    }
}