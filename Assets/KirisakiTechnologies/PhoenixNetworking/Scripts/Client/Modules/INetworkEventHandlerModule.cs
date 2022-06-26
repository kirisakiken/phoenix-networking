using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules
{
    public delegate void TcpReceiveEvent<in T>(T payload);
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
    }
}