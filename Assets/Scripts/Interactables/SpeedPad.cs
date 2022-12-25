using UnityEngine;

namespace Interactables {
    public class SpeedPad : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private float padForce = 10f;

        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
        }

        private void OnTriggerEnter(Collider other) {
            other.attachedRigidbody.AddForce(-transform.forward * padForce, ForceMode.Impulse);
        }
    }
}
