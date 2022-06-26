using System;

using KirisakiTechnologies.GameSystem.Scripts.Providers;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

using Newtonsoft.Json;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Providers
{
    public class TcpPacketProvider : GameProviderBaseMono, ITcpPacketProvider
    {
        #region ITcpPacketProvider Implementation

        public Packet ClientInitialConnectionPacket(TcpInitialConnectPayload payload) => BuildServerPacket(ServerPackets.ClientConnected, payload.ClientId, JsonConvert.SerializeObject(payload));

        public Packet ClientConnectReceivedBroadcastPacket(TcpConnectedClientBroadcastPayload payload) => BuildServerPacket(ServerPackets.ConnectedClientBroadcast, payload.ClientData.ClientId, JsonConvert.SerializeObject(payload));

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
            var packet = new Packet((int) packetId);
            packet.Write(message);
            packet.Write(clientId);

            return packet;
        }

        #endregion
    }
}