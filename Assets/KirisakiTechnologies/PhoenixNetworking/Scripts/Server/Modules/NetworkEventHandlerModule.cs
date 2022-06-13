using KirisakiTechnologies.GameSystem.Scripts.Modules;
using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules
{
    public class NetworkEventHandlerModule : GameModuleBaseMono, INetworkEventHandlerModule
    {
        public void ClientConnected(int clientId, Packet packet)
        {
            var receivedId = packet.ReadInt();
            var username = packet.ReadString();
            
            // TODO: add logic below to execute on client connected logic
            Debug.Log($"Message from client to server, ClientId: {receivedId} | Name: {username}");

            if (clientId != receivedId)
                Debug.LogError($"Client Id and received id does not match!!!");
            
            // TODO: broadcast connected client to others
        }
    }
}