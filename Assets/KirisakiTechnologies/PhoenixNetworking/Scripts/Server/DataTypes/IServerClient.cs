using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.DataTypes
{
    // TODO: add description
    public interface IServerClient
    {
        int Id { get; }

        string Name { get; set; }

        IServerTcp ServerTcp { get; }

        IServerUdp ServerUdp { get; }
    }

    // TODO: add description
    public interface IServerTcp
    {
        bool IsConnected { get; }

        int Id { get; }

        TcpClient Socket { get; }

        int DataBufferSize { get; }

        void Connect([NotNull] TcpClient socket);

        void SendData(Packet packet); // TODO: refactor packet file
    }

    // TODO: add description
    public interface IServerUdp
    {
        int Id { get; }

        IPEndPoint EndPoint { get; }

        void Connect([NotNull] IPEndPoint endPoint);

        void SendData(Packet packet);

        void HandleData(Packet packet);
    }
}