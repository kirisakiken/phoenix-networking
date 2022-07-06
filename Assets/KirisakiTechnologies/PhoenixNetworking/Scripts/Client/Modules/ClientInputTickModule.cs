using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Providers;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;

using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules
{
    public class ClientInputTickModule : GameModuleBaseMono, IClientInputTickModule
    {
        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _ClientModule = gameSystem.GetModule<IClientModule>();
            _NetworkEventHandlerModule = gameSystem.GetModule<INetworkEventHandlerModule>();
            _TcpPacketProvider = gameSystem.GetProvider<ITcpPacketProvider>();

            InitializeKeyStates();

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Private
        
        private readonly Dictionary<ClientInputKey, bool> _InputKeyStates = new Dictionary<ClientInputKey, bool>();
        private readonly Dictionary<ClientInputKey, bool> _PreviousKeyStates = new Dictionary<ClientInputKey, bool>();
        private readonly UdpClientInputPayload _ClientInputPayload = new UdpClientInputPayload
        {
            ModifiedKeys = new Dictionary<ClientInputKey, bool>(),
        };

        private IClientModule _ClientModule;
        private INetworkEventHandlerModule _NetworkEventHandlerModule;
        private ITcpPacketProvider _TcpPacketProvider;

        private bool CanSendClientInputTickPayload => _ClientModule?.Udp != null; // TODO: check if connection available instead null check

        private void ClientTick()
        {
            using (var packet = _TcpPacketProvider.UdpClientInputPacket(_ClientInputPayload))
                _NetworkEventHandlerModule.SendUdpDataToServer(packet);

            RefreshClientInputPayload();
        }

        private void CaptureInputKey(KeyCode keyCode, ClientInputKey clientInputKey)
        {
            if (!_PreviousKeyStates.ContainsKey(clientInputKey))
                throw new KeyNotFoundException($"Unable to find {nameof(ClientInputKey)}:{nameof(clientInputKey)} in collection {nameof(_PreviousKeyStates)}.");

            if (Input.GetKey(keyCode))
            {
                if (_PreviousKeyStates[clientInputKey])
                    return;

                _InputKeyStates.Add(clientInputKey, true);
                _PreviousKeyStates[clientInputKey] = true;
            }
            else
            {
                if (!_PreviousKeyStates[clientInputKey])
                    return;

                _InputKeyStates.Add(clientInputKey, false);
                _PreviousKeyStates[clientInputKey] = false;
            }
        }

        private void CaptureInputKeys()
        {
            RefreshInputKeyStates();

            // TODO: wrap key capture into loop
            CaptureInputKey(KeyCode.W, ClientInputKey.W);
            CaptureInputKey(KeyCode.A, ClientInputKey.A);
            CaptureInputKey(KeyCode.S, ClientInputKey.S);
            CaptureInputKey(KeyCode.D, ClientInputKey.D);

            BuildClientInputPayload();
        }

        private void InitializeKeyStates()
        {
            foreach (ClientInputKey keyValue in Enum.GetValues(typeof(ClientInputKey)))
            {
                _PreviousKeyStates.Add(keyValue, false);
            }
        }

        private void RefreshInputKeyStates()
        {
            _InputKeyStates.Clear();
        }

        private void RefreshClientInputPayload()
        {
            _ClientInputPayload.ModifiedKeys.Clear();
        }

        private void BuildClientInputPayload()
        {
            foreach (var keyState in _InputKeyStates)
            {
                if (_ClientInputPayload.ModifiedKeys.ContainsKey(keyState.Key))
                {
                    _ClientInputPayload.ModifiedKeys[keyState.Key] = keyState.Value;
                    continue;
                }

                _ClientInputPayload.ModifiedKeys.Add(keyState.Key, keyState.Value);
            }
        }

        #endregion

        #region MonoBehaviour Methods

        private void FixedUpdate()
        {
            if (!CanSendClientInputTickPayload)
                return;

            ClientTick();
        }

        private void Update()
        {
            // TODO: block capture if ui is focused
            if (!CanSendClientInputTickPayload)
                return;

            RefreshInputKeyStates();
            CaptureInputKeys();
        }

        #endregion
    }
}
