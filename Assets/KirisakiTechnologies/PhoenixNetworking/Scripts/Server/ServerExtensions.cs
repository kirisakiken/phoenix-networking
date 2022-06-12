using KirisakiTechnologies.PhoenixNetworking.CORE;
using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server
{
    public class ServerSend
    {
        private static void SendTcpData(int clientId, Packet packet)
        {
            packet.WriteLength();
            
            if (!Scripts.Server.Server.Clients.ContainsKey(clientId))
                Debug.LogError($"Unable to find client in collection Clients. Client Id: ${clientId}");

            Scripts.Server.Server.Clients[clientId].Tcp.SendData(packet);
        }

        private static void SendTcpDataToAll(Packet packet)
        {
            packet.WriteLength();

            foreach (var serverClient in Scripts.Server.Server.Clients.Values)
            {
                serverClient.Tcp.SendData(packet);
            }
        }

        private static void SendTcpDataToAllExceptOne(int clientId, Packet packet)
        {
            packet.WriteLength();

            foreach (var serverClient in Scripts.Server.Server.Clients.Values)
            {
                if (serverClient.Id == clientId)
                    continue;

                serverClient.Tcp.SendData(packet);
            }
        }
        
        public static void Welcome(int clientId, string message)
        {
            using (var packet = new Packet((int)ServerPackets.welcome))
            {
                packet.Write(message);
                packet.Write(clientId);

                SendTcpData(clientId, packet);
            }
        }
    }
}
