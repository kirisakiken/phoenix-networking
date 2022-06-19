using System.Collections.Generic;
using KirisakiTechnologies.GameSystem.Scripts.Providers;
using UnityEngine;

namespace KirisakiTechnologies.GameSystem.Scripts.Containers
{
    public class ProvidersContainerCollection : MonoBehaviour, IProvidersContainerCollection
    {
        #region IProviderContainerCollection Implementation

        public IReadOnlyCollection<IGameProvider> Providers => GetComponentsInChildren<IGameProvider>(false);

        #endregion
    }
}