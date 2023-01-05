using System;
using Network;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ball.PowerUps {
    [RequireComponent(typeof(PhotonView))]
    public class PowerUpBox : MonoBehaviour {
    
        [SerializeField] private bool debug;

        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;
            _logger.Log("Player touched.");

            var randomPowerUp = GetRandomPowerUp();
            var powerUpMan = other.GetComponent<PowerUpManager>();
            powerUpMan.AddPowerUp(randomPowerUp);
            _logger.Log("Applied "+randomPowerUp+" to player "+other.gameObject.name);
            
            NetworkManager.Destroy(gameObject);
        }
        
        /// <summary>
        /// Retrieves a randomly chosen power up
        /// </summary>
        private Type GetRandomPowerUp() {
            var powerUps = PowerUpManager.PowerUps.Values;
            var chosenPowerUpIndex = Random.Range(0, powerUps.Count);
            var i = 0;
            foreach (var powerUp in powerUps) {
                if (i == chosenPowerUpIndex) {
                    return powerUp;
                }
                i++;
            }

            throw new InvalidOperationException("Somehow, a chosen power up was not found");
        }
    }
}

