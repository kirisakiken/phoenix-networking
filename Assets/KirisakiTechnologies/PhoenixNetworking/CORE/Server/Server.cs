using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using KirisakiTechnologies.PhoenixNetworking.CORE.Client;
using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.CORE.Server
{
    public class Server : MonoBehaviour
    {
        public static Dictionary<int, ServerClient> Clients = new Dictionary<int, ServerClient>();
        public uint Port => _Port;

        public delegate void PacketHandler(int clientId, Packet packet);
        public static Dictionary<int, PacketHandler> _PacketHandlers;

        private void Start()
        {
            InitializeServerData();
            
            // initialize tcp listener
            _TcpListener = new TcpListener(IPAddress.Any, (int) Port);
            _TcpListener.Start();
            
            // start listening
            _TcpListener.BeginAcceptTcpClient(TcpConnectCallBack, null);
            
            Debug.Log($"Server started on port: ${Port}");
        }

        private void OnApplicationQuit()
        {
            _TcpListener.Stop();
        }

        private void TcpConnectCallBack(IAsyncResult result)
        {
            // get connected client
            var client = _TcpListener.EndAcceptTcpClient(result);
            
            // resume listening
            _TcpListener.BeginAcceptTcpClient(TcpConnectCallBack, null);
            
            Debug.Log($"Incoming connection from: ${client.Client.RemoteEndPoint} ...");
            
            // for each client, add connected client to collection
            // and listen
            for (var i = 1; i <= MaxClientCount; ++i)
            {
                // check if slot is empty
                if (Clients[i].Tcp.Socket == null)
                {
                    // populate empty slot with newly connected client
                    Clients[i].Tcp.Connect(client);
                    return;
                }
            }
            
            Debug.LogError($"{client.Client.RemoteEndPoint} failed to connect. Server full!");
        }


        [SerializeField]
        private uint _Port = 26950;

        private const int MaxClientCount = 20;

        private TcpListener _TcpListener;

        private static void InitializeServerData()
        {
            // initialize clients collection
            for (var i = 1; i <= MaxClientCount; ++i)
            {
                Clients.Add(i, new ServerClient((uint) i));
            }
            
            Debug.Log("Initialized clients collection");
            
            // initialize packet handlers
            _PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ClientPackets.welcomeReceived, ServerHandler.WelcomeReceived}
            };
            
            Debug.Log("Initialized server packet handlers");
        }
    }
}
