using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.Interactables {
    public class Bumper : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private float force = 2.5f;
        [SerializeField] private float minimumForce = 2f;

        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
        }

        private void OnCollisionEnter(Collision collision) {
            if (!collision.gameObject.CompareTag("Player")) return;
            var rb = collision.gameObject.GetComponent<Rigidbody>();
            var bounceForce = collision.impulse * force;
            _logger.Log("Player collided, applying force: "+bounceForce);
            if (bounceForce.magnitude < minimumForce) {
                bounceForce = bounceForce.normalized * minimumForce;
                _logger.Log("Force is too small! Instead, applying force: "+bounceForce);
            }
            rb.AddForce(bounceForce, ForceMode.Impulse);
        }
    }
}

