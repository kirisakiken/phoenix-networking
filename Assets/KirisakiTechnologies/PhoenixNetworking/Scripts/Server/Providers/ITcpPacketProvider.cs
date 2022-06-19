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
        Packet ClientConnectedPacket(int clientId, string message);

        /// <summary>
        ///     Packet to be build when client connect received (handshake)
        /// </summary>
        /// <remarks>
        ///     This must be disposed properly
        /// </remarks>
        Packet ClientConnectReceivedPacket(int clientId, string message);

        /// <summary>
        ///     Packet to be build when client connect received and information related to that
        ///     has to be broadcast
        /// </summary>
        /// <remarks>
        ///     This must be disposed properly
        /// </remarks>
        Packet ClientConnectReceivedBroadcastPacket(int clientId, string message);

        /// <summary>
        ///     Packet to be build when client disconnected
        /// </summary>
        /// <remarks>
        ///     This must be disposed properly
        /// </remarks>
        Packet ClientDisconnectedPacket(int clientId, string message);
    }
}