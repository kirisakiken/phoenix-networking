using System;
using UnityEngine;
using UnityEngine.UI;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client
{
    public class ClientHandle : MonoBehaviour
    {
        private static Text _Text;
        private void Awake()
        {
            _Text = GameObject.Find("PlayText").GetComponent<Text>();
        }

        public static void Welcome(Packet packet)
        {
            var message = packet.ReadString();
            var id = packet.ReadInt();

            Client.Id = id;
            
            // Received information on connection
            _Text.text = $"ReceivedId: {id} \nReceivedMessage: {message}";
            
            // send ClientConnected received packet
            ClientSend.WelcomeReceived();
            
            Debug.Log($"Client: Message received from server: ( {message} | {id} )");
        }
    }
}
