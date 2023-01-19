using UnityEngine;

namespace Interactables {
    public class Bumper : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private float force = 2.5f;

        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
        }

        private void OnCollisionEnter(Collision collision) {
            if (!collision.gameObject.CompareTag("Player")) return;
            var rb = collision.gameObject.GetComponent<Rigidbody>();
            var bounceForce = collision.impulse * force;
            _logger.Log("Player collided, applying force: "+bounceForce);
            rb.AddForce(bounceForce, ForceMode.Impulse);
        }
    }
}

