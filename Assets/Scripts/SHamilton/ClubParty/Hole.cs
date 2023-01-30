using SHamilton.ClubParty.Ball;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty {
    public class Hole : MonoBehaviour {
    
        [SerializeField] private bool debug;

        [SerializeField] private int par = 3;
        public int Par => par;
        public bool isCurrent;
        public GameObject hole;

        private Logger _logger;
        private HoleCollider[] _holeColliders;
	
        private void Start() {
            _logger = new(this, debug);

            var colliders = hole.GetComponentsInChildren<Collider>();
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
