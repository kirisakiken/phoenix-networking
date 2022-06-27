using System.Collections.Generic;

using KirisakiTechnologies.GameSystem.Scripts.Tools;

using UnityEngine;

namespace KirisakiTechnologies.GameSystem.Scripts.Containers
{
    public class ToolsContainerCollection : MonoBehaviour, IToolsContainerCollection
    {
        #region IToolsContainerCollection Implementation

        public IReadOnlyCollection<IGameTool> Tools => GetComponentsInChildren<IGameTool>(false);

        #endregion
    }
}
