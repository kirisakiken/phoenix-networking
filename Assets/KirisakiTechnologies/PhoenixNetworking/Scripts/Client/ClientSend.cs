using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client
{
    public class ClientSend : MonoBehaviour
    {
        private static void SendTcpData(Packet packet)
        {
            packet.WriteLength();
            Scripts.Client.Client.Tcp.SendData(packet);
        }

        public static void WelcomeReceived()
        {
            using (var packet = new Packet((int)ClientPackets.ConnectReceived))
            {
                packet.Write(Scripts.Client.Client.Id);
                packet.Write(Scripts.Client.Client.Name);
                
                SendTcpData(packet);
            }
        }
    }
}
