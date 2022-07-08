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

        public Packet ClientMessageBroadcastPacket(TcpClientMessagePayload payload) => BuildServerPacket(ServerPackets.TcpMessagePayloadReceived, payload.ClientData.ClientId, JsonConvert.SerializeObject(payload));

        public Packet UdpServerTickPacket(UdpServerTickPayload payload) => BuildServerPacket(ServerPackets.UdpServerTick, JsonConvert.SerializeObject(payload));

        public UdpClientInputPayload DeserializeUdpClientInputPayloadPacket(Packet packet)
        {
            var message = packet.ReadString();

            var payload = JsonConvert.DeserializeObject<UdpClientInputPayload>(message);
            if (payload == null)
                throw new InvalidOperationException($"Failed to deserialize payload in {nameof(DeserializeUdpClientInputPayloadPacket)}");

            return payload;
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

        private Packet BuildServerPacket(ServerPackets packetId, string message)
        {
            var packet = new Packet((int) packetId);
            packet.Write(message);

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