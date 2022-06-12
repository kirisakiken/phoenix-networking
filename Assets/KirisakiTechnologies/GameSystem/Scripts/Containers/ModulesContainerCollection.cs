using System.Collections.Generic;

using KirisakiTechnologies.GameSystem.Scripts.Modules;

using UnityEngine;

namespace KirisakiTechnologies.GameSystem.Scripts.Containers
{
    public class ModulesContainerCollection : MonoBehaviour, IModulesContainerCollection
    {
        public IReadOnlyCollection<IGameModule> Modules => GetComponentsInChildren<IGameModule>(false);
    }
}