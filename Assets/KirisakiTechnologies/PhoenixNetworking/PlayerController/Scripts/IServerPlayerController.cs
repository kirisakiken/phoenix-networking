using KirisakiTechnologies.GameSystem.Scripts.Controllers;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Entities.Player;

namespace KirisakiTechnologies.PhoenixNetworking.PlayerController.Scripts
{
    public interface IServerPlayerController : IGameController
    {
        IPlayerEntity PlayerEntity { get; set; }
    }
}
