using System.Collections.Generic;
using System.Net;

using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Server.DataTypes;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules
{
    public delegate void PacketHandler(int clientId, Packet packet); // TODO: refactor packet file

    public delegate void NetworkEvent(int clientId);
    public delegate void PacketEvent(int clientId, Packet packet);

    /// <summary>
    ///     Represents Main TCP/UDP ServerModule
    /// </summary>
    public interface IServerModule : IGameModule
    {
        /// <summary>
        ///     Invoked on incoming connection clientId is assigned by server
        /// </summary>
        event NetworkEvent OnClientConnected;

        /// <summary>
        ///     Invoked on response from client on incoming connection (handshake)
        /// </summary>
        event PacketEvent OnClientConnectionHandshakeCompleted;

        /// <summary>
        ///     
        /// </summary>
        event PacketEvent OnClientTcpMessagePayloadReceived;

        /// <summary>
        /// 
        /// </summary>
        event PacketEvent OnClientUdpPayloadReceived;

        /// <summary>
        ///     Represents Clients Id/ServerClient Collection
        /// </summary>
        IReadOnlyDictionary<int, IServerClient> Clients { get; }

        /// <summary>
        ///     Represents PacketHandler Collection
        ///     TODO: refactor key type (int to enum)
        /// </summary>
        IReadOnlyDictionary<int, PacketHandler> PacketHandlers { get; }

        /// <summary>
        ///     Sends given packet via UDP protocol
        /// </summary>
        void SendUdpData(IPEndPoint clientEndPoint, Packet packet);
    }
}