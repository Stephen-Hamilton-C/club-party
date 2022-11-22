using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Ball.PowerUps {
    public class PowerUpManager : MonoBehaviour {

        [SerializeField] private bool debug;
        
        private readonly HashSet<Type> _activePowerUps = new();
        private Logger _logger;

        private void Awake() {
            _logger = new(this, debug);
        }

        public bool AddPowerUp(Type powerUpType) {
            if (!powerUpType.IsSubclassOf(typeof(PowerUp)))
                return false;
            
            if (_activePowerUps.Contains(powerUpType))
                return false;

            gameObject.AddComponent(powerUpType);
            _activePowerUps.Add(powerUpType);
            return true;
        }

        public bool RemovePowerUp(Type powerUpType) {
            if (!powerUpType.IsSubclassOf(typeof(PowerUp)))
                return false;
            
            if (!_activePowerUps.Contains(powerUpType))
                return false;

            if (TryGetComponent(powerUpType, out Component powerUp)) {
                Destroy(powerUp);
            }

            return _activePowerUps.Remove(powerUpType);
        }
        
    }
}
