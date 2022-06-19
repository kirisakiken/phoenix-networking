using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client
{
    public class ClientHandle : MonoBehaviour
    {
        public static void Welcome(Packet packet)
        {
            var message = packet.ReadString();
            var id = packet.ReadInt();

            Debug.Log($"{Client.Id} | {id}");
            Client.Id = id;
            // send ClientConnected received packet
            ClientSend.WelcomeReceived();
            
            Debug.Log($"Client: Message received from server: ( {message} | {id} )");
        }
    }
}
