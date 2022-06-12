using System;
using System.Threading.Tasks;

using UnityEngine;

namespace KirisakiTechnologies.GameSystem.Modules
{
    public abstract class GameModuleBaseMono : MonoBehaviour, IGameModule
    {
        public virtual Task Initialize(IGameSystem gameSystem)
        {
            if (gameSystem == null)
                throw new ArgumentNullException(nameof(gameSystem));

            return Task.CompletedTask;
        }

        public virtual Task Begin(IGameSystem gameSystem)
        {
            if (gameSystem == null)
                throw new ArgumentNullException(nameof(gameSystem));

            return Task.CompletedTask;
        }
    }
}