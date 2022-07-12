using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Entities;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Entities.Player;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules.Entities;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Providers;

using Newtonsoft.Json;
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
            _NetworkEntitiesModule = gameSystem.GetModule<INetworkEntitiesModule>();
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
        private INetworkEntitiesModule _NetworkEntitiesModule;
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
            // TODO: find a way to exclude unmodified properties of network entities (for network optimization)
            foreach (var networkEntityState in _NetworkEntitiesModule.NetworkEntities)
            {
                var networkEntity = BuildNetworkEntity(networkEntityState.Key);
                switch (networkEntityState.Value)
                {
                    case NetworkEntityState.Unchanged:
                        continue;
                    case NetworkEntityState.Added:
                    {
                        _Payload.AddedEntities.Add(networkEntity);
                        break;
                    }
                    case NetworkEntityState.Modified:
                    {
                        _Payload.ModifiedEntities.Add(networkEntity);
                        break;
                    }
                    case NetworkEntityState.Removed:
                    {
                        _Payload.RemovedEntities.Add(networkEntity);
                        break;
                    }
                }
            }

            _NetworkEntitiesModule.CleanStates();
        }

        private static GenericNetworkEntity BuildNetworkEntity(INetworkEntity networkEntity)
        {
            var genericNetworkEntity = new GenericNetworkEntity();
            switch (networkEntity)
            {
                case IPlayerEntity playerEntity:
                {
                    genericNetworkEntity.EntityType = EntityType.PlayerEntity;
                    genericNetworkEntity.Data = JsonConvert.SerializeObject(new PlayerNetworkEntity
                    {
                        NetworkId = playerEntity.NetworkId,
                        Position = new Point3D
                        {
                            X = playerEntity.Position.x,
                            Y = playerEntity.Position.y,
                            Z = playerEntity.Position.z,
                        },
                        Rotation = new Point4D
                        {
                            X = playerEntity.Rotation.x,
                            Y = playerEntity.Rotation.y,
                            Z = playerEntity.Rotation.z,
                            W = playerEntity.Rotation.w,
                        },
                    });

                    return genericNetworkEntity;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(networkEntity));
            }
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
