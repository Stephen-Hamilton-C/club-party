using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.Interactables {
    public class Bumper : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private float force = 7.5f;

        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
        }

        private void OnCollisionEnter(Collision collision) {
            if (!collision.gameObject.CompareTag("Player")) return;
            var rb = collision.gameObject.GetComponent<Rigidbody>();
            // var bounceForce = collision.impulse.normalized * force;
            var bounceDirection = collision.rigidbody.position - transform.position;
            bounceDirection.y = 0;
            var bounceForce = bounceDirection.normalized * force;
            _logger.Log("Player collided, applying force: "+bounceForce);
            rb.AddForce(bounceForce, ForceMode.Impulse);
        }
    }
}

