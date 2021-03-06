using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Controllers;
using KirisakiTechnologies.GameSystem.Scripts.Entities;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Modules.Entities;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Entities.Player;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules;

using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.PlayerController.Scripts
{
    public class ServerPlayerController : GameControllerBaseMono, IServerPlayerController
    {
        #region IServerPlayerController Implementation

        public IPlayerEntity PlayerEntity { get; set; }

        #endregion

        #region Overrides

        public override void Initialize(IGameSystem gameSystem)
        {
            base.Initialize(gameSystem);

            _EntitiesModule = gameSystem.GetModule<IEntitiesModule>();
            _ServerClientsInputModule = gameSystem.GetModule<IServerClientsInputModule>();
        }

        #endregion

        #region Private

        [SerializeField]
        private Transform _RootTransform;

        [SerializeField]
        private int _MovementSpeed = 10;

        private IEntitiesModule _EntitiesModule;
        private IServerClientsInputModule _ServerClientsInputModule;

        private void MovePlayer()
        {
            if (!_ServerClientsInputModule.ClientInputs.ContainsKey(PlayerEntity.ClientId))
                return;

            var clientInputs = _ServerClientsInputModule.ClientInputs[PlayerEntity.ClientId];
            var movementVector = Vector3.zero;
            // TODO: remap input enum to vector 3 for finding input direction (e.g. forward/back)
            foreach (var input in clientInputs)
            {
                switch (input.Key)
                {
                    case ClientInputKey.W:
                    {
                        var val = input.Value == true
                            ? 1
                            : 0;

                        movementVector.x += val;
                        break;
                    }
                    case ClientInputKey.S:
                    {
                        var val = input.Value == true
                            ? 1
                            : 0;

                        movementVector.x -= val;
                        break;
                    }
                    case ClientInputKey.A:
                    {
                        var val = input.Value == true
                            ? 1
                            : 0;

                        movementVector.z += val;
                        break;
                    }
                    case ClientInputKey.D:
                    {
                        var val = input.Value == true
                            ? 1
                            : 0;

                        movementVector.z -= val;
                        break;
                    }
                }
            }

            movementVector *= _MovementSpeed;
            _RootTransform.Translate(movementVector * Time.deltaTime);

            PlayerEntity.Position = _RootTransform.position;
            PlayerEntity.Rotation = _RootTransform.rotation;

            if (!movementVector.IsZero()) // can update entity
            {
                _EntitiesModule.UpdateEntities(new EntitiesTransaction
                {
                    ModifiedEntities = { PlayerEntity },
                });
            }
        }

        #endregion

        #region MonoBehaviour Methods

        private void Update()
        {
            MovePlayer();
        }

        #endregion
    }
}
