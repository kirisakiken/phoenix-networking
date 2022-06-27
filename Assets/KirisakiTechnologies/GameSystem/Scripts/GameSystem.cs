using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts.Containers;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.GameSystem.Scripts.Providers;
using KirisakiTechnologies.GameSystem.Scripts.Tools;

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

        public IGameProvider GetProvider(Type providerType) => GetOptionalProvider(providerType) ?? throw new InvalidOperationException($"Provider type: {providerType.Name} not available");

        public IGameProvider GetOptionalProvider(Type providerType) => _GameProviders.FirstOrDefault(providerType.IsInstanceOfType);

        public IGameTool GetTool(Type toolType) => GetOptionalTool(toolType) ?? throw new InvalidOperationException($"Tool type: {toolType.Name} not available");

        public IGameTool GetOptionalTool(Type toolType) => _GameTools.FirstOrDefault(toolType.IsInstanceOfType);

        public async Task InitializeAndBegin()
        {
            _ModulesContainerCollection = GetComponentInChildren<IModulesContainerCollection>(); // TODO: ? Possible better way??
            _ProvidersContainerCollection = GetComponentInChildren<IProvidersContainerCollection>();
            _ToolsContainerCollection = GetComponentInChildren<IToolsContainerCollection>();

            await InitializeAndBeginSystem(this);
        }

        #endregion

        #region Private

        [SerializeField] private InitializationMethod _InitializationMethod;

        private IModulesContainerCollection _ModulesContainerCollection;
        private IProvidersContainerCollection _ProvidersContainerCollection;
        private IToolsContainerCollection _ToolsContainerCollection;

        private readonly List<IGameModule> _GameModules = new List<IGameModule>();
        private readonly List<IGameProvider> _GameProviders = new List<IGameProvider>();
        private readonly List<IGameTool> _GameTools = new List<IGameTool>();

        private async Task InitializeAndBeginSystem(IGameSystem system)
        {
            await Initialize(system);
            await Begin(system);
        }

        private async Task Initialize(IGameSystem system)
        {
            PopulateGameModules();
            PopulateGameProviders();
            PopulateGameTools();

            foreach (var module in _GameModules)
                await module.Initialize(system);

            foreach (var provider in _GameProviders)
                await provider.Initialize(system);

            foreach (var tool in _GameTools)
                await tool.Initialize(system);
        }

        private async Task Begin(IGameSystem system)
        {
            foreach (var module in _GameModules)
                await module.Begin(system);

            foreach (var provider in _GameProviders)
                await provider.Begin(system);

            foreach (var tool in _GameTools)
                await tool.Begin(system);
        }

        private void PopulateGameModules()
        {
            if (_GameModules.Count > 0)
                return;

            if (_ModulesContainerCollection == null)
                throw new NullReferenceException($"{nameof(_ModulesContainerCollection)} is not provided");

            var modules = _ModulesContainerCollection.Modules;
            _GameModules.AddRange(modules);
        }

        private void PopulateGameProviders()
        {
            if (_GameProviders.Count > 0)
                return;

            if (_ProvidersContainerCollection == null)
                throw new NullReferenceException($"{nameof(_ProvidersContainerCollection)} is not provided");

            var providers = _ProvidersContainerCollection.Providers;
            _GameProviders.AddRange(providers);
        }

        private void PopulateGameTools()
        {
            if (_GameTools.Count > 0)
                return;

            if (_ToolsContainerCollection == null)
                throw new NullReferenceException($"{nameof(_ToolsContainerCollection)} is not provided");

            var tools = _ToolsContainerCollection.Tools;
            _GameTools.AddRange(tools);
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
