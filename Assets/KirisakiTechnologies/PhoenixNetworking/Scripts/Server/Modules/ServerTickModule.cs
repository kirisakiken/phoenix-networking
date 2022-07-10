using System.Collections.Generic;
using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Providers;

using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules
{
    public class ServerTickModule : GameModuleBaseMono, IServerTickModule
    {
        #region IServerTickModule Implementation

        public bool CanExecuteTick => _CanExecuteTick; // TODO: implement modify (e.g. on udp established)

        public int TickRate => _TickRate;

        #endregion

        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _NetworkEventHandlerModule = gameSystem.GetModule<INetworkEventHandlerModule>();
            _TcpPacketProvider = gameSystem.GetProvider<ITcpPacketProvider>();

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Private

        [SerializeField]
        private bool _CanExecuteTick;

        [SerializeField]
        private int _TickRate = 60;

        private INetworkEventHandlerModule _NetworkEventHandlerModule;
        private ITcpPacketProvider _TcpPacketProvider;

        private readonly UdpServerTickPayload _Payload = new UdpServerTickPayload
        {
            AddedEntities = new List<GenericNetworkEntity>(),
            ModifiedEntities = new List<GenericNetworkEntity>(),
            RemovedEntities = new List<GenericNetworkEntity>(),
        };

        private void Tick()
        {
            if (!CanExecuteTick)
                return;

            RefreshPayload();
            BuildPayload();

            using (var packet = _TcpPacketProvider.UdpServerTickPacket(_Payload))
                _NetworkEventHandlerModule.SendUdpDataToAll(packet);
        }

        private void RefreshPayload()
        {
            _Payload.AddedEntities.Clear();
            _Payload.ModifiedEntities.Clear();
            _Payload.RemovedEntities.Clear();
        }

        private void BuildPayload()
        {
            // TODO: implement
            // get added, modified and removed entities
            // add those to payload (excluding unmodified properties of those)
        }

        #endregion

        #region MonoBehaviour Methods

        // TODO: move from fixed update to another thread and execute it using fixed rate
        private void FixedUpdate()
        {
            Tick();
        }

        #endregion
    }
}
