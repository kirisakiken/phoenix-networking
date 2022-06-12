using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KirisakiTechnologies.GameSystem.Scripts.Containers;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using UnityEngine;

namespace KirisakiTechnologies.GameSystem.Scripts
{
    public enum InitializationMethod
    {
        OnAwake,
        OnStart,
    }

    public class GameSystem : MonoBehaviour, IGameSystem
    {
        #region IGameSystem Implementation

        public IGameModule GetModule(Type moduleType) => GetOptionalModule(moduleType) ?? throw new InvalidOperationException($"Module type: {moduleType.Name} not available");

        public IGameModule GetOptionalModule(Type moduleType) => _GameModules.FirstOrDefault(moduleType.IsInstanceOfType);

        public async Task InitializeAndBegin()
        {
            _ModulesContainerCollection = GetComponentInChildren<IModulesContainerCollection>(); // TODO: ? Possible better way??

            await InitializeAndBeginModules(this);
        }

        #endregion

        #region Private

        [SerializeField] private InitializationMethod _InitializationMethod;

        private IModulesContainerCollection _ModulesContainerCollection;

        private readonly List<IGameModule> _GameModules = new List<IGameModule>();

        private async Task InitializeAndBeginModules(IGameSystem system)
        {
            PopulateGameModules();

            foreach (var module in _GameModules)
                await module.Initialize(system);

            foreach (var module in _GameModules)
                await module.Begin(system);
        }

        private void PopulateGameModules()
        {
            if (_GameModules.Count > 0)
                return;

            if (_ModulesContainerCollection == null)
                throw new InvalidOperationException($"{nameof(_ModulesContainerCollection)} is not provided");

            var modules = _ModulesContainerCollection.Modules;
            _GameModules.AddRange(modules);
        }

        #endregion

        #region MonoBehaviour Methods

        private async void Awake()
        {
            if (_InitializationMethod != InitializationMethod.OnAwake)
                return;

            await InitializeAndBegin();
        }

        private async void Start()
        {
            if (_InitializationMethod != InitializationMethod.OnStart)
                return;

            await InitializeAndBegin();
        }

        #endregion
    }
}
