using System.Net.Sockets;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.DataTypes
{
    // TODO: add description
    public interface IServerClient
    {
        int Id { get; }

        IServerTcp ServerTcp { get; }
    }

    // TODO: add description
    public interface IServerTcp
    {
        int Id { get; }

        TcpClient Socket { get; }

        int DataBufferSize { get; }

        void Connect(TcpClient socket);

        void SendData(Packet packet); // TODO: refactor packet file
    }

    // TODO: implementation
    public interface IServerUdp { }
}