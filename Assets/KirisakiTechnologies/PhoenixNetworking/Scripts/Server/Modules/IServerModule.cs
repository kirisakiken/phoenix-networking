using System.Collections.Generic;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Server.DataTypes;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules
{
    public delegate void PacketHandler(int clientId, Packet packet); // TODO: refactor packet file

    // TODO: add description
    public interface IServerModule : IGameModule
    {
        IReadOnlyDictionary<int, IServerClient> Clients { get; }

        IReadOnlyDictionary<int, PacketHandler> PacketHandlers { get; }

        void SendTcpData(int clientId, Packet packet);

        void SendTcpDataToAll(Packet packet);

        void SendTcpDataToAllExceptOne(int clientId, Packet packet);
    }
}