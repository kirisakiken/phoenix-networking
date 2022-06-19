using System.Collections.Generic;
using KirisakiTechnologies.GameSystem.Scripts.Providers;

namespace KirisakiTechnologies.GameSystem.Scripts.Containers
{
    /// <summary>
    ///     Represents a container that holds ownership of providers
    /// </summary>
    public interface IProvidersContainerCollection
    {
        /// <summary>
        ///     Represents providers collection that belongs to the container
        /// </summary>
        IReadOnlyCollection<IGameProvider> Providers { get; }
    }
}