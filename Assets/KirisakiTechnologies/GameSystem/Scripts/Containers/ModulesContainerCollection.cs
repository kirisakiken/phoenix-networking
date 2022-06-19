using System.Collections.Generic;

using KirisakiTechnologies.GameSystem.Scripts.Modules;

using UnityEngine;

namespace KirisakiTechnologies.GameSystem.Scripts.Containers
{
    public class ModulesContainerCollection : MonoBehaviour, IModulesContainerCollection
    {
        #region IModulesContainerCollection Implementation

        public IReadOnlyCollection<IGameModule> Modules => GetComponentsInChildren<IGameModule>(false);

        #endregion
    }
}