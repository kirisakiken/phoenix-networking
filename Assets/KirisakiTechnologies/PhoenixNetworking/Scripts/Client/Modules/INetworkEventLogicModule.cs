using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules
{
    /// <summary>
    ///     Represents generic network logic event
    /// </summary>
    public delegate void NetworkLogicEvent<in T>(T payload); // TODO: where : T is Payload

    /// <summary>
    ///     Responsible of handling logic needs to be executed
    ///     on various network events
    ///     TODO: can this module be removed? And services that subs to this module can sub to INetworkEventHandlerModule?
    /// </summary>
    public interface INetworkEventLogicModule : IGameModule
    {
        /// <summary>
        ///     Event that needs to be invoked when network event logic module
        ///     receives TcpInitialConnectPayload
        /// </summary>
        event NetworkLogicEvent<TcpInitialConnectPayload> OnTcpInitialConnectPayloadReceived;

        /// <summary>
        ///     Event that needs to be invoked when connected client broadcast
        ///     payload is received
        /// </summary>
        event NetworkLogicEvent<TcpConnectedClientBroadcastPayload> OnTcpConnectedClientBroadcastPayloadReceived;

        /// <summary>
        ///     Event that needs to be invoked when tcp client message payload
        ///     is received
        /// </summary>
        event NetworkLogicEvent<TcpClientMessagePayload> OnTcpClientMessagePayloadReceived;

        /// <summary>
        ///     Event that needs to be invoked when udp payload is received
        /// </summary>
        event NetworkLogicEvent<UdpPayload> OnUdpPayloadReceived; 

        /// <summary>
        ///     Event that needs to be invoked when udp server tick payload is received
        /// </summary>
        event NetworkLogicEvent<UdpServerTickPayload> OnUdpServerTickReceived; 

        /// <summary>
        ///     Event that needs to be invoked when server requests handshake package
        /// </summary>
        event NetworkLogicEvent<int> OnHandshakePacketRequested; // TODO convert in T from int to Payload type
    }
}