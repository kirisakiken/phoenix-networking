using System.Threading.Tasks;
using JetBrains.Annotations;

namespace KirisakiTechnologies.GameSystem.Scripts.Providers
{
    public interface IGameProvider
    {
        /// <summary>
        ///     Initializes the provider
        /// </summary>
        Task Initialize([NotNull] IGameSystem gameSystem);

        /// <summary>
        ///     Begins the provider
        /// </summary>
        Task Begin([NotNull] IGameSystem gameSystem);
    }
}