﻿using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules
{
    /// <summary>
    ///     Represents generic network logic event
    /// </summary>
    public delegate void NetworkLogicEvent<in T>(T payload);

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
    }
}