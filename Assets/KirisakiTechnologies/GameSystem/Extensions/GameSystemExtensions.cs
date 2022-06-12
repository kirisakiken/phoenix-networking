using System;

using KirisakiTechnologies.GameSystem.Modules;

namespace KirisakiTechnologies.GameSystem.Extensions
{
    /// <summary>
    ///     Game System Extensions
    /// </summary>
    public static class GameSystemExtensions
    {
        /// <summary>
        ///     Gets and returns given type of game module. Throws if fails to find
        /// </summary>
        public static T GetModule<T>(this IGameSystem self) where T : class, IGameModule
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            return (self.GetModule(typeof(T)) as T)!;
        }

        /// <summary>
        ///     Gets and returns given type of game module. Returns null if fails to find
        /// </summary>
        public static T GetOptionalModule<T>(this IGameSystem self) where T : class, IGameModule
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            return self.GetOptionalModule(typeof(T)) as T;
        }
    }
}