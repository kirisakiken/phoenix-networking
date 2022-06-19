using System;
using System.Threading.Tasks;

using JetBrains.Annotations;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.GameSystem.Scripts.Providers;

namespace KirisakiTechnologies.GameSystem.Scripts
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
        ///     Tries to find provider with given type. Throws if fails
        /// </summary>
        IGameProvider GetProvider([NotNull] Type type);

        /// <summary>
        ///     Tries to find provider with given type. Returns null if fails
        /// </summary>
        IGameProvider GetOptionalProvider([NotNull] Type type);

        /// <summary>
        ///     Initializes and begins the Game System
        /// </summary>
        Task InitializeAndBegin();
    }
}