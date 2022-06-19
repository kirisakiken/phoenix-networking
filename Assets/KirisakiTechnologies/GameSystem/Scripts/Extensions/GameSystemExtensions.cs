using System;
using System.Threading.Tasks;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.GameSystem.Scripts.Providers;

namespace KirisakiTechnologies.GameSystem.Scripts.Extensions
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

        /// <summary>
        ///     Gets and returns given type of game provider. Throws if fails to find
        /// </summary>
        public static T GetProvider<T>(this IGameSystem self) where T : class, IGameProvider
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            return (self.GetProvider(typeof(T)) as T)!;
        }

        /// <summary>
        ///     Gets and returns given type of game provider. Returns null if fails to find
        /// </summary>
        public static T GetOptionalProvider<T>(this IGameSystem self) where T : class, IGameProvider
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            return self.GetOptionalProvider(typeof(T)) as T;
        }
    }
}