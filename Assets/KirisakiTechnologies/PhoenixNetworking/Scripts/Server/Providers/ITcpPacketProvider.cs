using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Providers
{
    /// <summary>
    ///     Packet builder/provider for various network events
    /// </summary>
    public interface ITcpPacketProvider : IPacketProvider
    {
        /// <summary>
        ///     Packet to be build when client connected (initial connect)
        /// </summary>
        /// <remarks>
        ///     This must be disposed properly
        /// </remarks>
        Packet ClientInitialConnectionPacket(TcpInitialConnectPayload payload);

        /// <summary>
        ///     Packet to be build when client connect received and information related to that
        ///     has to be broadcast
        /// </summary>
        /// <remarks>
        ///     This must be disposed properly
        /// </remarks>
        Packet ClientConnectReceivedBroadcastPacket(TcpConnectedClientBroadcastPayload payload);

        /// <summary>
        ///     Packet to be build when client disconnected
        /// </summary>
        /// <remarks>
        ///     This must be disposed properly
        /// </remarks>
        Packet ClientDisconnectedPacket(int clientId, string message);

        /// <summary>
        ///     Packet that needs to be broadcast when client sends message to server
        /// </summary>
        /// <remarks>
        ///     This must be disposed properly
        /// </remarks>
        Packet ClientMessageBroadcastPacket(TcpClientMessagePayload payload);
    }
}