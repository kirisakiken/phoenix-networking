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

        /// <summary>
        ///     Represents the function that needs to be executed
        ///     every fixed frame rate. Main usage of this method
        ///     is sending UDP payload to clients
        /// </summary>
        void Tick();
    }
}
