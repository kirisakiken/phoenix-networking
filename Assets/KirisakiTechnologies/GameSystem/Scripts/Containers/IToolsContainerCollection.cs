using System.Collections.Generic;

using KirisakiTechnologies.GameSystem.Scripts.Tools;

namespace KirisakiTechnologies.GameSystem.Scripts.Containers
{
    /// <summary>
    ///     Represents a container that holds ownership of tools
    /// </summary>
    public interface IToolsContainerCollection
    {
        /// <summary>
        ///     Represents modules collection that belongs to the container
        /// </summary>
        IReadOnlyCollection<IGameTool> Tools { get; }
    }
}
