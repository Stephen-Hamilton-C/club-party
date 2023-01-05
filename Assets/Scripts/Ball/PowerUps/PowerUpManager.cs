using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

namespace Ball.PowerUps {
    [RequireComponent(typeof(PhotonView))]
    public class PowerUpManager : MonoBehaviour {

        private const int MaxPowerUps = 3;

        public static readonly Dictionary<string, Type> PowerUps = new() {
            { typeof(HyperBall).ToString(), typeof(HyperBall) }
        };

        [SerializeField] private bool debug;

        // TODO: There's a critical issue with this design.
        // We need data for the power up to display it correctly, but can't create it because it would apply effects
        // Maybe the Type is of some PowerUpData class with a default constructor.
        // This PowerUpData class would hold attributes like name, description, offensive, and the component Type
        public IReadOnlyList<Type> StoredPowerUps => _storedPowerUps;
        public IReadOnlyCollection<Type> ActivePowerUps => _activePowerUps;

        private Logger _logger;
        private PhotonView _view;
        private readonly List<Type> _storedPowerUps = new();
        private readonly HashSet<Type> _activePowerUps = new();

        private void Start() {
            _logger = new(this, debug);
            _view = GetComponent<PhotonView>();
        }

        public void AddPowerUp(Type powerUpType) {
            ValidatePowerUpType<PowerUp>(powerUpType);
            if (_storedPowerUps.Count < MaxPowerUps) {
                _storedPowerUps.Add(powerUpType);
                _logger.Log("Added " + powerUpType + " to stored power ups");
            } else {
                _logger.Log("Would've added " + powerUpType + ", but power up storage is full.");
            }
        }

        public bool RemovePowerUp(Type powerUpType) {
            ValidatePowerUpType<PowerUp>(powerUpType);
            return _storedPowerUps.Remove(powerUpType);
        }

        public bool ActivatePowerUp(Type powerUpType) {
            ValidatePowerUpType<PowerUp>(powerUpType);
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

            if (powerUpType.IsSubclassOf(typeof(PlayerPowerUp))) {
                if (_activePowerUps.Contains(powerUpType)) {
                    _logger.Warn("PowerUp of this type (" + powerUpName + ") has already been applied.");
                    return;
                }

                _activePowerUps.Add(powerUpType);
                gameObject.AddComponent(powerUpType);
                _logger.Log("Added PowerUp to the character as a component (" + powerUpType + ")");
            } else if (powerUpType.IsSubclassOf(typeof(EnvironmentPowerUp))) {
                var environmentContainer = new GameObject(powerUpName);
                environmentContainer.AddComponent(powerUpType);
                _logger.Log("Added PowerUp to a new empty GameObject");
            }
        }

        public void PowerUpFinished(Type powerUpType) {
            ValidatePowerUpType<PlayerPowerUp>(powerUpType);
            _logger.Log("Powerup finished running ("+powerUpType+")");
            _view.RPC("PowerUpFinishedRPC", RpcTarget.AllBuffered, powerUpType.ToString());
        }

        [PunRPC, UsedImplicitly]
        private void PowerUpFinishedRPC(string powerUpName) {
            _logger.Log("Received RPC for PowerUpFinished with "+powerUpName);
            var powerUpType = PowerUps[powerUpName];
            _activePowerUps.Remove(powerUpType);
        }

        private void ValidatePowerUpType<T>(Type powerUpType) where T : PowerUp {
            if (!powerUpType.IsSubclassOf(typeof(T)))
                throw new ArgumentException("Type must be a subclass of "+typeof(T).Name+"!");
        }

    }
}

