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

        [Tooltip("The lowest bounds of each axis the player can go")]
        [SerializeField] private Vector3 minimumPosition = new(-1000f, -10f, -1000f);

        [Tooltip("The highest bounds of each axis the player can go")]
        [SerializeField] private Vector3 maximumPosition = new(1000f, 1000f, 1000f);

        private Rigidbody _rb;
        private Vector3 _respawnPoint;
        private Logger _logger;
        private bool _respawning;
        /// <summary>
        /// Switched to true if SetRespawnPoint was called while _respawning was true
        /// </summary>
        private bool _shouldSetSpawnPoint;

        /// <summary>
        /// Sets the respawn point to the current position
        /// </summary>
        public void SetRespawnPoint() {
            if (_respawning) {
                _logger.Log("Attempt to set spawnpoint while respawning. This call will be delayed by two FixedUpdates.");
                _shouldSetSpawnPoint = true;
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
            if (_shouldSetSpawnPoint)
                SetRespawnPoint();
        }

        private void HoleFinished() {
            _respawning = true;
            _shouldSetSpawnPoint = true;
            StartCoroutine(FinishSpawning());
        }

        private void Awake() {
            _logger = new(this, debug);
            _rb = GetComponent<Rigidbody>();
            
            GameManager.OnHoleFinished += HoleFinished;
        }

        private void Start() {
            SetRespawnPoint();
        }

        private void OnDestroy() {
            GameManager.OnHoleFinished -= HoleFinished;
        }

        private void FixedUpdate() {
            if (_rb.position.x < minimumPosition.x || _rb.position.x > maximumPosition.x ||
                _rb.position.y < minimumPosition.y || _rb.position.y > maximumPosition.y ||
                _rb.position.z < minimumPosition.z || _rb.position.z > maximumPosition.z)
            {
                Respawn();
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Respawn")) {
                Respawn();
            }
        }
    }
}
