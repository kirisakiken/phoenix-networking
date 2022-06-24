using System.Collections.Generic;
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
        ///     Represents TCP instance of the client
        /// </summary>
        IClientTcp Tcp { get; }

        /// <summary>
        ///     
        /// </summary>
        IReadOnlyDictionary<int, PacketHandler> PacketHandlers { get; }

        /// <summary>
        ///     
        /// </summary>
        void ConnectToServer();
    }

    // TODO: add descriptions
    public interface IClientTcp
    {
        /// <summary>
        ///     
        /// </summary>
        TcpClient Socket { get; }

        /// <summary>
        ///     
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        ///     
        /// </summary>
        void Connect();

        /// <summary>
        ///     
        /// </summary>
        void Disconnect();

        /// <summary>
        ///     
        /// </summary>
        void SendData(Packet packet);
    }
}