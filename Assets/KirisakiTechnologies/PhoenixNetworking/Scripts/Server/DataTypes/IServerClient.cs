using System.Net.Sockets;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.DataTypes
{
    // TODO: add description
    public interface IServerClient
    {
        int Id { get; }

        string Name { get; set; }

        IServerTcp ServerTcp { get; }
    }

    // TODO: add description
    public interface IServerTcp
    {
        bool IsConnected { get; }

        int Id { get; }

        TcpClient Socket { get; }

        int DataBufferSize { get; }

        void Connect(TcpClient socket);

        void SendData(Packet packet); // TODO: refactor packet file
    }

    // TODO: implementation
    public interface IServerUdp { }
}