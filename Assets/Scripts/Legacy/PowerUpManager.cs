using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Legacy {
    /// <summary>
    /// Simply holds the collection of currently active PowerUps for this player
    /// </summary>
    [Obsolete("To be replaced with new PowerUp design")]
    public class PowerUpManager : MonoBehaviour {

        [SerializeField] private bool debug;
        
        private readonly HashSet<PowerUp> _activePowerUps = new();
        private Logger _logger;

        private void Awake() {
            _logger = new(this, debug);
        }

        /// <summary>
        /// Adds a PowerUp to the player
        /// </summary>
        /// <param name="powerUp">The PowerUp to add</param>
        /// <returns>True if the PowerUp was added, false if the PowerUp type has already been applied</returns>
        public bool AddPowerUp(PowerUp powerUp) {
            var powerUpType = powerUp.GetType();
            foreach (var activePowerUp in _activePowerUps) {
                if (activePowerUp.GetType() == powerUpType)
                    return false;
            }

            return _activePowerUps.Add(powerUp);
        }

        /// <summary>
        /// Removes a PowerUp from the player
        /// </summary>
        /// <param name="powerUp">The PowerUp to remove</param>
        /// <returns>Whether a PowerUp that matched the given PowerUp's type was removed</returns>
        public bool RemovePowerUp(PowerUp powerUp) {
            if (!_activePowerUps.Remove(powerUp)) {
                var powerUpType = powerUp.GetType();
                foreach (var activePowerUp in _activePowerUps) {
                    if (activePowerUp.GetType() == powerUpType)
                        return _activePowerUps.Remove(activePowerUp);
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets an array copy of the currently active PowerUps
        /// </summary>
        /// <returns>An array copy of PowerUps</returns>
        public PowerUp[] GetPowerUps() {
            return _activePowerUps.ToArray();
        }

    }
}
