using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules
{
    public delegate void TcpReceiveEvent<in T>(T payload);
    // public delegate void TcpReceiveEvent<in T>(T payload) where T : Payload;

    /// <summary>
    ///     Holds ownership of various network events
    /// </summary>
    public interface INetworkEventHandlerModule : IGameModule
    {
        /// <summary>
        ///     Invoked when initial connect package received
        ///     Subscribers are expected to handle/execute required logic
        ///     on initial connect
        /// </summary>
        event TcpReceiveEvent<TcpInitialConnectPayload> OnInitialConnectPackageReceived;

        /// <summary>
        ///     Invoked when broadcast message of connected client received
        ///     Subscribers are expected to handle/execute required logic
        ///     on receiving connected client payload
        /// </summary>
        event TcpReceiveEvent<TcpConnectedClientBroadcastPayload> OnClientConnectedBroadcastPackageReceived;

        /// <summary>
        ///     Invoked when a client sends message to server and server
        ///     broadcasts that message to available clients
        ///     Subscribers are expected to handle/execute logic
        ///     on receiving message from other clients
        /// </summary>
        event TcpReceiveEvent<TcpClientMessagePayload> OnClientTcpMessagePayloadPackageReceived;

        /// <summary>
        ///     Invoked when UDP payload received from server
        ///     Subscribers are expected to handle/execute logic
        ///     on receiving UDP payload from server
        /// </summary>
        event TcpReceiveEvent<UdpPayload> OnUdpPayloadReceived;

        /// <summary>
        ///     Invoked when UDP Server tick received from server
        ///     Subscribers are expected to handle/execute logic
        ///     on receiving server tick. E.g. update entities,
        ///     update time.
        /// </summary>
        event TcpReceiveEvent<UdpServerTickPayload> OnUdpServerTickReceived;

        /// <summary>
        ///     Invoked when client connected to server
        ///     and server requests handshake information
        ///     Subscribers are expected to handle/execute
        ///     handshake logic and responsible of sending
        ///     initial handshake information to server (e.g. clientId, nickName)
        /// </summary>
        event TcpReceiveEvent<int> OnHandshakePacketRequested; // TODO: convert in T from int to Payload type

        /// <summary>
        ///     Used to send any packet to server via TCP protocol
        /// </summary>
        void SendTcpDataToServer(Packet packet);

        /// <summary>
        ///     Used to send any packet to server via UDP protocol
        /// </summary>
        void SendUdpDataToServer(Packet packet);

        /// <summary>
        ///     Used to send tcp client message to server
        /// </summary>
        void SendTcpClientMessageToServer(int clientId, string message);

        /// <summary>
        ///     Used to send udp client message to server
        /// </summary>
        void SendUdpClientMessageToServer(int clientId, string message);
    }
}