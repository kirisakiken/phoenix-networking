using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server
{
    public class ServerHandler : MonoBehaviour
    {
        public static void WelcomeReceived(int clientId, Packet packet)
        {
            var receivedId = packet.ReadInt();
            var username = packet.ReadString();
            
            Debug.Log($"Message from client to server, ClientId: {receivedId} | Name: {username}");

            if (clientId != receivedId)
                Debug.LogError($"Client Id and received id does not match!!!");
            
            // TODO: broadcast connected client to others
        }
    }
}
