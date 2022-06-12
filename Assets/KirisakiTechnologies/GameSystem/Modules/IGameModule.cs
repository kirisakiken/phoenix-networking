using System.Threading.Tasks;

using JetBrains.Annotations;

namespace KirisakiTechnologies.GameSystem.Modules
{
    /// <summary>
    ///     Represents a generic game module
    /// </summary>
    public interface IGameModule
    {
        /// <summary>
        ///     Initializes the module
        /// </summary>
        Task Initialize([NotNull] IGameSystem gameSystem);

        /// <summary>
        ///     Begins the module
        /// </summary>
        Task Begin([NotNull] IGameSystem gameSystem);
    }
}