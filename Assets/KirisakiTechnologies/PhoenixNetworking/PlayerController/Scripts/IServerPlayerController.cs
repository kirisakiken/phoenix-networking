using KirisakiTechnologies.GameSystem.Scripts.Controllers;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Entities.Player;

namespace KirisakiTechnologies.PhoenixNetworking.PlayerController.Scripts
{
    /// <summary>
    ///     Represents a server player controller that is responsible of consuming incoming
    ///     client input and executing controller actions using client input
    /// </summary>
    public interface IServerPlayerController : IGameController
    {
        /// <summary>
        ///     Player Entity reference of the controller
        /// </summary>
        IPlayerEntity PlayerEntity { get; set; }
    }
}
