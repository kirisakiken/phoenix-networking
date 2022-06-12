using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client
{
    public class ClientHandle : MonoBehaviour
    {
        public static void Welcome(Packet packet)
        {
            var message = packet.ReadString();
            var id = packet.ReadInt();

            Scripts.Client.Client.Id = id;
            
            // send welcome received packet
            ClientSend.WelcomeReceived();
            
            Debug.Log($"Message from server: {message} | {id}");
        }
    }
}
