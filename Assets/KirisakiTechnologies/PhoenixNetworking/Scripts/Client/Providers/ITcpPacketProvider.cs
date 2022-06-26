﻿using KirisakiTechnologies.GameSystem.Scripts.Providers;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

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
        TcpInitialConnectPayload DeserializeOnClientInitialConnectionPacket(Packet packet, out int receivedId);

        /// <summary>
        ///     Received broadcast message from server when another client connects
        /// </summary>
        TcpConnectedClientBroadcastPayload DeserializeOnClientConnectedBroadcastReceivedPacket(Packet packet, out int receivedClientId);

        /// <summary>
        ///     Packet needs to be build to send welcome received information packet
        /// </summary>
        Packet OnConnectWelcomeReceivedPacket(int clientId, string message);
    }
}