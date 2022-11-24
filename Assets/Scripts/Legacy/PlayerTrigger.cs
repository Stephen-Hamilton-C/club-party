using System;
using UnityEngine;
using UnityEngine.Events;

namespace Legacy {
    [Serializable]
    public class PlayerTriggerEvent : UnityEvent<GameObject> { }

    [Obsolete("Replaced with HoleTrigger")]
    public class PlayerTrigger : MonoBehaviour {

        [SerializeField] private bool debug;
        private Logger _logger;

        public PlayerTriggerEvent onPlayerEntered = new();
        public PlayerTriggerEvent onPlayerExited = new();

        void Awake() {
            _logger = new(this);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                _logger.Log("A Player entered this trigger.");
                onPlayerEntered.Invoke(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) {
                _logger.Log("A Player left this trigger.");
                onPlayerExited.Invoke(other.gameObject);
            }
        }
    }
}
