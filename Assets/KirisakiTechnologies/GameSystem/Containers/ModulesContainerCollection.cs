using System.Collections.Generic;

using KirisakiTechnologies.GameSystem.Modules;

using UnityEngine;

namespace KirisakiTechnologies.GameSystem.Containers
{
    public class ModulesContainerCollection : MonoBehaviour, IModulesContainerCollection
    {
        public IReadOnlyCollection<IGameModule> Modules => GetComponentsInChildren<IGameModule>(false);
    }
}