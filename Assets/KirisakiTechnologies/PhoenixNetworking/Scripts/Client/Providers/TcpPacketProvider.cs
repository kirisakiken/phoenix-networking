using System;

using KirisakiTechnologies.GameSystem.Scripts.Providers;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

using Newtonsoft.Json;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Providers
{
    // TODO: rename to NetworkPacketProvider
    public class TcpPacketProvider : GameProviderBaseMono, ITcpPacketProvider
    {
        // TODO: add JsonSerializerSettings and implementations in methods
        #region ITcpPacketProvider Implementation

        public TcpInitialConnectPayload DeserializeOnClientInitialConnectionPacket(Packet packet)
        {
            var message = packet.ReadString();

            var payload = JsonConvert.DeserializeObject<TcpInitialConnectPayload>(message);
            if (payload == null)
                throw new InvalidOperationException($"Failed to deserialize payload in {nameof(DeserializeOnClientInitialConnectionPacket)}");

            return payload;
        }

        public TcpConnectedClientBroadcastPayload DeserializeOnClientConnectedBroadcastReceivedPacket(Packet packet)
        {
            var message = packet.ReadString();

            var payload = JsonConvert.DeserializeObject<TcpConnectedClientBroadcastPayload>(message);
            if (payload == null)
                throw new InvalidOperationException($"Failed to deserialize payload in {nameof(DeserializeOnClientConnectedBroadcastReceivedPacket)}");

            return payload;
        }

        public TcpClientMessagePayload DeserializeOnClientTcpMessagePayloadReceivedPacket(Packet packet)
        {
            var message = packet.ReadString();

            var payload = JsonConvert.DeserializeObject<TcpClientMessagePayload>(message);
            if (payload == null)
                throw new InvalidOperationException($"Failed to deserialize payload in {nameof(DeserializeOnClientTcpMessagePayloadReceivedPacket)}");

            return payload;
        }

        public UdpPayload DeserializeOnUdpPayloadReceivedPacket(Packet packet)
        {
            var message = packet.ReadString();

            var payload = JsonConvert.DeserializeObject<UdpPayload>(message);
            if (payload == null)
                throw new InvalidOperationException($"Failed to deserialize payload in {nameof(DeserializeOnUdpPayloadReceivedPacket)}");

            return payload;
        }

        public UdpServerTickPayload DeserializeOnUdpServerTickReceivedPacket(Packet packet)
        {
            var message = packet.ReadString();

            var payload = JsonConvert.DeserializeObject<UdpServerTickPayload>(message);
            if (payload == null)
                throw new InvalidOperationException($"Failed to deserialize payload in {nameof(DeserializeOnUdpServerTickReceivedPacket)}");

            return payload;
        }

        public Packet OnConnectWelcomeReceivedPacket(int clientId, string message) => BuildClientPacket(ClientPackets.ConnectReceived, clientId, message);


        public Packet TcpClientMessagePacket(int clientId, string message) => BuildClientPacket(ClientPackets.TcpMessagePayloadReceived, clientId, message);

        public Packet UdpClientMessagePacket(string message) => BuildClientUdpPacket(ClientPackets.UdpTestReceive, message);

        public Packet UdpClientInputPacket(UdpClientInputPayload payload) => BuildClientUdpPacket(ClientPackets.UdpClientInputTickReceived, JsonConvert.SerializeObject(payload));

        #endregion

        #region Private

        private Packet BuildClientPacket(ClientPackets packetId, int clientId, string message)
        {
            var packet = new Packet((int) packetId);
            packet.Write(clientId);
            packet.Write(message);

            return packet;
        }

        private Packet BuildClientUdpPacket(ClientPackets packetId, string message)
        {
            var packet = new Packet((int)packetId);
            packet.Write(message);

            return packet;
        }

        #endregion
    }
}