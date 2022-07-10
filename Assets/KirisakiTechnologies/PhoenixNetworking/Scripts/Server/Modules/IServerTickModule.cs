using KirisakiTechnologies.GameSystem.Scripts.Modules;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules
{
    /// <summary>
    ///     Represents the server tick module
    /// </summary>
    public interface IServerTickModule : IGameModule
    {
        /// <summary>
        ///     True if Tick should be available for execution,
        ///     false otherwise
        /// </summary>
        bool CanExecuteTick { get; }

        /// <summary>
        ///     Represents the tick rate of the module
        /// </summary>
        int TickRate { get; }
    }
}
