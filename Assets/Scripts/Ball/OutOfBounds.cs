using Photon.Pun;
using UnityEngine;

namespace Ball {
    /// <summary>
    /// Teleports the player back to spawn if they fall off the map
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class OutOfBounds : MonoBehaviour {

        [SerializeField] private bool debug;
        [Tooltip("The lowest Y the player can go")]
        [SerializeField] private float minimumY = -25f;

        private Rigidbody _rb;
        private Vector3 _respawnPoint;
        private Logger _logger;

        /// <summary>
        /// Sets the respawn point to the current position
        /// </summary>
        public void SetRespawnPoint() {
            _respawnPoint = transform.position;
            _logger.Log("Reset respawn point to "+_respawnPoint);
        }

        /// <summary>
        /// Respawns the player to the respawn point
        /// </summary>
        public void Respawn() {
            _rb.velocity = Vector3.zero;
            _rb.position = _respawnPoint;
            _logger.Log("Resetting position to " + _respawnPoint);
        }

        private void Awake() {
            _logger = new(this, debug);
            _rb = GetComponent<Rigidbody>();
            
            SetRespawnPoint();
        }

        private void FixedUpdate() {
            if (_rb.position.y < minimumY)
                Respawn();
        }
    
    }
}
