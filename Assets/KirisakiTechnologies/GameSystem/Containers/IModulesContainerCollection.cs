using System.Collections.Generic;

using KirisakiTechnologies.GameSystem.Modules;

namespace KirisakiTechnologies.GameSystem.Containers
{
    /// <summary>
    ///     Represents a container that holds ownership of modules
    /// </summary>
    public interface IModulesContainerCollection
    {
        /// <summary>
        ///     Represents modules collection that belongs to the container
        /// </summary>
        IReadOnlyCollection<IGameModule> Modules { get; }
    }
}