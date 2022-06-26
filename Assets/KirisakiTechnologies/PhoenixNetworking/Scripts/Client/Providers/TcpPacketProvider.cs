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

        // TODO: exporting receivedId with out keyword is not good implementation. Find better way e.g. data structures
        public TcpInitialConnectPayload DeserializeOnClientInitialConnectionPacket(Packet packet, out int receivedId) // TODO: change return type to data structure
        {
            var message = packet.ReadString();
            var id = packet.ReadInt();

            receivedId = id;

            var payload = JsonConvert.DeserializeObject<TcpInitialConnectPayload>(message);
            if (payload == null)
                throw new InvalidOperationException($"Failed to deserialize payload in {nameof(DeserializeOnClientInitialConnectionPacket)}");

            return payload;
        }

        // TODO: exporting receivedId with out keyword is not good implementation. Find better way e.g. data structures
        public TcpConnectedClientBroadcastPayload DeserializeOnClientConnectedBroadcastReceivedPacket(Packet packet, out int receivedClientId) // TODO: change return type to data structure
        {
            var message = packet.ReadString();
            var id = packet.ReadInt();

            receivedClientId = id;

            var payload = JsonConvert.DeserializeObject<TcpConnectedClientBroadcastPayload>(message);
            if (payload == null)
                throw new InvalidOperationException($"Failed to deserialize payload in {nameof(DeserializeOnClientConnectedBroadcastReceivedPacket)}");

            return payload;
        }

        public Packet OnConnectWelcomeReceivedPacket(int clientId, string message) => BuildClientPacket(ClientPackets.ConnectReceived, clientId, message);

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