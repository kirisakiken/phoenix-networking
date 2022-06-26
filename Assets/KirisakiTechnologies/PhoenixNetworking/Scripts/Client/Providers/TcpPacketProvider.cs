using System;
using KirisakiTechnologies.GameSystem.Scripts.Providers;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;
using Newtonsoft.Json;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Providers
{
    public class TcpPacketProvider : GameProviderBaseMono, ITcpPacketProvider
    {
        // TODO: add JsonSerializerSettings and implementations in methods
        #region ITcpPacketProvider Implementation

        public TcpInitialConnectPayload DeserializeOnClientInitialConnectionPacket(Packet packet) // TODO: change return type to data structure
        {
            var message = packet.ReadString();

            var payload = JsonConvert.DeserializeObject<TcpInitialConnectPayload>(message);
            if (payload == null)
                throw new InvalidOperationException($"Failed to deserialize payload in {nameof(DeserializeOnClientInitialConnectionPacket)}");

            return payload;
        }

        public TcpConnectedClientBroadcastPayload DeserializeOnClientConnectedBroadcastReceivedPacket(Packet packet) // TODO: change return type to data structure
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

        // TODO: refactor with json data structures
        public Packet OnConnectWelcomeReceivedPacket(int clientId, string message) => BuildClientPacket(ClientPackets.ConnectReceived, clientId, message);

        // TODO: refactor with json data structures
        public Packet TcpClientMessagePacket(int clientId, string message) => BuildClientPacket(ClientPackets.TcpMessagePayloadReceived, clientId, message);

        #endregion

        #region Private

        private Packet BuildClientPacket(ClientPackets packetId, int clientId, string message)
        {
            var packet = new Packet((int) packetId);
            packet.Write(clientId);
            packet.Write(message);

            return packet;
        }

        #endregion
    }
}