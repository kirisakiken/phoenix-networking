using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using KirisakiTechnologies.GameSystem.Modules;

namespace KirisakiTechnologies.GameSystem
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameSystem
    {
        /// <summary>
        ///     Tries to find module with given type. Throws if fails
        /// </summary>
        IGameModule GetModule([NotNull] Type type);

        /// <summary>
        ///     Tries to find module with given type. Returns null if fails
        /// </summary>
        IGameModule GetOptionalModule([NotNull] Type type);

        /// <summary>
        ///     Initializes and begins the Game System
        /// </summary>
        public Task InitializeAndBegin();
    }
}