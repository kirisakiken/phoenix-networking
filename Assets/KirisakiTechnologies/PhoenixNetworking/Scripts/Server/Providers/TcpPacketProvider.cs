﻿using System;

using KirisakiTechnologies.GameSystem.Scripts.Providers;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Providers
{
    public class TcpPacketProvider : GameProviderBaseMono, ITcpPacketProvider
    {
        #region ITcpPacketProvider Implementation

        public Packet ClientConnectedPacket(int clientId, string message) => BuildServerPacket(ServerPackets.ClientConnected, clientId, message);

        public Packet ClientConnectReceivedPacket(int clientId, string message) => BuildServerPacket(ServerPackets.ClientConnected, clientId, message);

        public Packet ClientConnectReceivedBroadcastPacket(int clientId, string message)
        {
            throw new NotImplementedException();
        }

        public Packet ClientDisconnectedPacket(int clientId, string message)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private

        private Packet BuildServerPacket(ServerPackets packetId, int clientId, string message)
        {
            var packet = new Packet((int) packetId);
            packet.Write(message);
            packet.Write(clientId);

            return packet;
        }

        private Packet BuildClientPacket(ClientPackets packetId, int clientId, string message)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}