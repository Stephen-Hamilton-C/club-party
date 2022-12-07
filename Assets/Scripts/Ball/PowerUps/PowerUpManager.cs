using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Legacy;
using Photon.Pun;
using UnityEngine;

namespace Ball.PowerUps {
    [RequireComponent(typeof(PhotonView))]
    public class PowerUpManager : MonoBehaviour {

        public static readonly Dictionary<string, Type> PowerUps = new() {
            { typeof(HyperBall).ToString(), typeof(HyperBall) }
        };

        [SerializeField] private bool debug;

        public IReadOnlyList<Type> StoredPowerUps => _storedPowerUps;
        public IReadOnlyList<Type> ActivePowerUps => _activePowerUps;

        private Logger _logger;
        private PhotonView _view;
        private readonly List<Type> _storedPowerUps = new();
        private readonly List<Type> _activePowerUps = new();

        private void Start() {
            _logger = new(this, debug);
            _view = GetComponent<PhotonView>();
        }

        public void AddPowerUp(Type powerUpType) {
            ValidatePowerUpType(powerUpType);
            _storedPowerUps.Add(powerUpType);
            _logger.Log("Added "+powerUpType+" to stored power ups");
        }

        public bool ActivatePowerUp(Type powerUpType) {
            ValidatePowerUpType(powerUpType);
            if (_storedPowerUps.Remove(powerUpType)) {
                // Actually adding the component should be networked as it is possible for other players to apply
                // power-ups to this player
                _logger.Log("Powerup successfully removed, activating powerup across all clients...");
                _view.RPC("ActivatePowerUpRPC", RpcTarget.AllBuffered, powerUpType.ToString());
                return true;
            }

            _logger.Log("Powerup was not stored, will not activate.");
            return false;
        }

        [PunRPC, UsedImplicitly]
        private void ActivatePowerUpRPC(string powerUpName) {
            _logger.Log("Received RPC for ActivatePowerUp with "+powerUpName);
            var powerUpType = PowerUps[powerUpName];
            if (_activePowerUps.Contains(powerUpType)) {
                _logger.Warn("PowerUp of this type ("+powerUpName+") has already been applied.");
                return;
            }
            
            _activePowerUps.Add(powerUpType);
            gameObject.AddComponent(powerUpType);
            _logger.Log("Added PowerUp to the character as a component ("+powerUpType+")");
        }

        public void PowerUpFinished(Type powerUpType) {
            ValidatePowerUpType(powerUpType);
            _logger.Log("Powerup finished running ("+powerUpType+")");
            _view.RPC("PowerUpFinishedRPC", RpcTarget.AllBuffered, powerUpType.ToString());
        }

        [PunRPC, UsedImplicitly]
        private void PowerUpFinishedRPC(string powerUpName) {
            _logger.Log("Received RPC for PowerUpFinished with "+powerUpName);
            var powerUpType = PowerUps[powerUpName];
            _activePowerUps.Remove(powerUpType);
        }

        private void ValidatePowerUpType(Type powerUpType) {
            if (!powerUpType.IsSubclassOf(typeof(PowerUp)))
                throw new ArgumentException("Type must be a subclass of PowerUp!");
        }

    }
}

