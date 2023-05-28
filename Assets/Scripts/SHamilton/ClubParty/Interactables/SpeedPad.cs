using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.Interactables {
    public class SpeedPad : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private float padForce = 10f;
        [SerializeField] private float correctionForceFactor = 0.3f;

        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
        }

        private void OnTriggerEnter(Collider other) {
            var plrVelocity = other.attachedRigidbody.velocity;
            var padDirection = -transform.forward;

            var projectionVelOnDir = Vector3.Dot(plrVelocity, padDirection) * padDirection;
            var rejectionVelOnDir = plrVelocity - projectionVelOnDir;
            var correctionForce = rejectionVelOnDir * correctionForceFactor;

            other.attachedRigidbody.AddForce(padDirection * padForce - correctionForce, ForceMode.Impulse);
        }
    }
}
