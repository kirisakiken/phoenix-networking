using KirisakiTechnologies.PhoenixNetworking.CORE.Server;
using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.CORE.Client
{
    public class ClientSend : MonoBehaviour
    {
        private static void SendTcpData(Packet packet)
        {
            packet.WriteLength();
            Client.Tcp.SendData(packet);
        }

        public static void WelcomeReceived()
        {
            using (var packet = new Packet((int)ClientPackets.welcomeReceived))
            {
                packet.Write(Client.Id);
                packet.Write(Client.Name);
                
                SendTcpData(packet);
            }
        }
    }
}
