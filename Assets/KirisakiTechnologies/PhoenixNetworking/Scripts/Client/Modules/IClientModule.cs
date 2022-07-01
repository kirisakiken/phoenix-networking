using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using KirisakiTechnologies.GameSystem.Scripts.Modules;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules
{
    public delegate void PacketHandler(Packet packet);

    public delegate void NetworkEvent(int clientId);
    public delegate void PacketEvent(Packet packet);

    /// <summary>
    ///     Represents Main TCP/UDP ClientModule
    /// </summary>
    public interface IClientModule : IGameModule
    {
        /// <summary>
        ///     Invoked when client connects to server and server sends initial connect package to client
        /// </summary>
        event PacketEvent OnClientConnected;

        /// <summary>
        ///     
        /// </summary>
        event PacketEvent OnClientConnectBroadcastReceived;

        /// <summary>
        ///     
        /// </summary>
        event PacketEvent OnClientTcpMessagePayloadReceived;

        /// <summary>
        ///     Client ID
        /// </summary>
        int Id { get; set; }

        /// <summary>
        ///     IP
        /// </summary>
        string Ip { get; }

        /// <summary>
        ///     Port
        /// </summary>
        int Port { get; }

        /// <summary>
        ///     Represents TCP instance of the client
        /// </summary>
        IClientTcp Tcp { get; }

        /// <summary>
        ///     Represents UDP instance of the client
        /// </summary>
        IClientUdp Udp { get; }

        /// <summary>
        ///     
        /// </summary>
        IReadOnlyDictionary<int, PacketHandler> PacketHandlers { get; }

        /// <summary>
        ///     Connects to server
        ///     TODO: deprecate
        /// </summary>
        void ConnectToServer();

        /// <summary>
        ///     Connects to server with given nickname,
        ///     ip and port
        /// </summary>
        void ConnectToServer(string nickName, string ip, uint port);

        /// <summary>
        ///     Disconnects from server
        /// </summary>
        void DisconnectFromServer();
    }

    /// <summary>
    ///     Represents a client TCP data class
    ///     Responsible of having definition of connect/disconnect and send data methods
    /// </summary>
    public interface IClientTcp
    {
        /// <summary>
        ///     TCP Socket of the Client
        /// </summary>
        TcpClient Socket { get; }

        /// <summary>
        ///     Connection state of the client
        ///     True if connected, false otherwise
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        ///     Establishes a TCP connection to server
        /// </summary>
        void Connect();

        /// <summary>
        ///     Disconnects from server if connected
        /// </summary>
        void Disconnect();

        /// <summary>
        ///     Writes given packet to server via TCP
        /// </summary>
        void SendData(Packet packet);
    }

    /// <summary>
    ///     Represents a client UDP data class
    ///     Responsible of having definition of connect/disconnect and send data methods
    /// </summary>
    public interface IClientUdp
    {
        /// <summary>
        ///     UDP Socket of the Client
        /// </summary>
        UdpClient Socket { get; }

        /// <summary>
        ///     IP Endpoint of UDP connection
        /// </summary>
        IPEndPoint EndPoint { get; }

        /// <summary>
        ///     Establishes a UDP connection to server with given local port
        /// </summary>
        void Connect(int localPort);

        /// <summary>
        ///     Writes given packet to server via UDP
        /// </summary>
        void SendData(Packet packet);
    }
}