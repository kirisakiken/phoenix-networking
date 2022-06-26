using System;
using KirisakiTechnologies.GameSystem.Scripts.Providers;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Providers
{
    public class TcpPacketProvider : GameProviderBaseMono, ITcpPacketProvider
    {
        #region ITcpPacketProvider Implementation

        // TODO: exporting receivedId with out keyword is not good implementation. Find better way e.g. data structures
        public string DeserializeOnClientInitialConnectionPacket(Packet packet, out int receivedId) // TODO: change return type to data structure
        {
            var message = packet.ReadString();
            var id = packet.ReadInt();

            receivedId = id;
            return $"Received ID: {id} | Message: {message}";
        }

        // TODO: exporting receivedId with out keyword is not good implementation. Find better way e.g. data structures
        public string DeserializeOnClientConnectedBroadcastReceivedPacket(Packet packet, out int receivedClientId) // TODO: change return type to data structure
        {
            var message = packet.ReadString();
            var id = packet.ReadInt();

            receivedClientId = id;
            return $"Received Connected Client ID: {receivedClientId} | Broadcast Message: {message}";
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