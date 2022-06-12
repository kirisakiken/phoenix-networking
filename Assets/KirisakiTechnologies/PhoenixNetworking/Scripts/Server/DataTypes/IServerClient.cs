using System.Net.Sockets;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.DataTypes
{
    // TODO: add description
    public interface IServerClient
    {
        int Id { get; }
        
        IServerTcp ServerTcp { get; }
    }
    
    public interface IServerTcp
    {
        int Id { get; }
        
        TcpClient Socket { get; }
        
        int DataBufferSize { get; }
        
        void Connect(TcpClient socket);
    }

    // TODO: implementation
    public interface IServerUdp
    {
        
    }
}