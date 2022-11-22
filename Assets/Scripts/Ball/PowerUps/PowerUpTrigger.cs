using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ball.PowerUps {
    public class PowerUpTrigger : MonoBehaviour {

        private enum PowerUpType {
            HyperBall
        }

        [SerializeField] private bool debug;
        [SerializeField] private PowerUpType powerUp;

        private Logger _logger;

        private Dictionary<PowerUpType, Type> _powerUps = new() {
            { PowerUpType.HyperBall, typeof(HyperBall) }
        };

        private void Start() {
            _logger = new(this, debug);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject playerObject) { 
            var manager = playerObject.GetComponent<PowerUpManager>();
            manager.AddPowerUp(_powerUps[powerUp]);
            Destroy(gameObject);
        }
    }
}
