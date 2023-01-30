using System.Collections;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.Ball {
    /// <summary>
    /// Teleports the player back to spawn if they fall off the map
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class OutOfBounds : MonoBehaviour {

        [SerializeField] private bool debug;
        [Tooltip("The lowest Y the player can go")]
        [SerializeField] private float minimumY = -25f;
        [Tooltip("The highest Y the player can go")]
        [SerializeField] private float maximumY = 1000f;

        private Rigidbody _rb;
        private Vector3 _respawnPoint;
        private Logger _logger;
        private bool _respawning;
        /// <summary>
        /// Switched to true if SetRespawnPoint was called while _respawning was true
        /// </summary>
        private bool _setSpawn;

        /// <summary>
        /// Sets the respawn point to the current position
        /// </summary>
        public void SetRespawnPoint() {
            if (_respawning) {
                _logger.Log("Attempt to set spawnpoint while respawning. This call will be delayed by two FixedUpdates.");
                _setSpawn = true;
                return;
            }
            _respawnPoint = transform.position;
            _logger.Log("Reset respawn point to "+_respawnPoint);
        }

        /// <summary>
        /// Respawns the player to the respawn point
        /// </summary>
        public void Respawn() {
            _respawning = true;
            _rb.position = _respawnPoint;
            _rb.velocity = Vector3.zero;
            _logger.Log("Resetting position to " + _respawnPoint);
            StartCoroutine(FinishSpawning());
        }

        private IEnumerator FinishSpawning() {
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            _respawning = false;
            if (_setSpawn)
                SetRespawnPoint();
        }

        private void Awake() {
            _logger = new(this, debug);
            _rb = GetComponent<Rigidbody>();
            
            GameManager.OnHoleFinished += SetRespawnPoint;
        }

        private void Start() {
            SetRespawnPoint();
        }

        private void OnDestroy() {
            GameManager.OnHoleFinished -= SetRespawnPoint;
        }

        private void FixedUpdate() {
            if (_rb.position.y < minimumY || _rb.position.y > maximumY)
                Respawn();
        }
    
    }
}
