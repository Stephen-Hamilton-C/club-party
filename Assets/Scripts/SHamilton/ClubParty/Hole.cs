using SHamilton.ClubParty.Ball;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty {
    public class Hole : MonoBehaviour {
    
        [SerializeField] private bool debug;

        [SerializeField] private int par = 3;
        public int Par => par;
        public bool isCurrent;
        [Tooltip("The root GameObject for the hole")]
        public GameObject map;
        [Tooltip("The hole GameObject in the map")]
        public GameObject hole;
        [Tooltip("Where the player should spawn when starting the hole")]
        public Transform spawn;

        private Logger _logger;
        private HoleCollider[] _holeColliders;
	
        private void Start() {
            _logger = new(this, debug);

            map = gameObject;

            if (spawn == null) {
                _logger.Warn("Spawn was not set! Will attempt to automatically find the spawn by looking for " +
                             "a GameObject called \"Spawn\"");
                spawn = map.transform.Find("Spawn");
            }
            
            // Check that hole exists
            if (hole == null) {
                // Try to automatically find the hole
                _logger.Warn("Hole was not set! Will attempt to automatically find the hole by looking for " +
                             "a GameObject called \"Hole\"");
                hole = map.transform.Find("Hole")?.gameObject;
            }
            
            // Populate colliders
            var colliders = map.GetComponentsInChildren<Collider>();
            _holeColliders = new HoleCollider[colliders.Length];
            var i = 0;
            _logger.Log("Found "+colliders.Length+" colliders in hole.");
            foreach (var coll in colliders) {
                var holeCollider = coll.gameObject.AddComponent<HoleCollider>();
                holeCollider.debug = debug;
                holeCollider.OnCharacterCollided += HoleTouched;
                _holeColliders[i] = holeCollider;
                _logger.Log("Created HoleCollider");
                i++;
            }
        }

        private void OnDestroy() {
            foreach (var holeCollider in _holeColliders) {
                holeCollider.OnCharacterCollided -= HoleTouched;
            }
        }

        private void HoleTouched(GameObject character) {
            _logger.Log("Hole has been touched by "+character.name);
            if (isCurrent) return;
            _logger.Log("Hole is not current. Respawning player...");
            character.GetComponent<OutOfBounds>().Respawn();
        }
    }
}
