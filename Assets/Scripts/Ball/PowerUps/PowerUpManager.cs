using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Ball.PowerUps {
    public class PowerUpManager : MonoBehaviour {

        [SerializeField] private bool debug;
        
        private readonly HashSet<PowerUp> _activePowerUps = new();
        private Logger _logger;

        private void Awake() {
            _logger = new(this, debug);
        }

        public bool AddPowerUp(PowerUp powerUp) {
            return _activePowerUps.Add(powerUp);
        }

        public bool RemovePowerUp(PowerUp powerUp) {
            return _activePowerUps.Remove(powerUp);
        }

    }
}
