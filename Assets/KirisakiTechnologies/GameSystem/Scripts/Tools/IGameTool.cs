using System.Threading.Tasks;

using JetBrains.Annotations;

namespace KirisakiTechnologies.GameSystem.Scripts.Tools
{
    /// <summary>
    ///     Represents a game tool that is responsible of various
    ///     UI related actions
    /// </summary>
    public interface IGameTool
    {
        /// <summary>
        ///     Initializes the tool
        /// </summary>
        Task Initialize([NotNull] IGameSystem gameSystem);

        /// <summary>
        ///     Begins the tool
        /// </summary>
        Task Begin([NotNull] IGameSystem gameSystem);
    }
}
