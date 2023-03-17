using Photon.Pun;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty {
    /// <summary>
    /// Listens for players who have touched this hole, then informs the GameManager
    /// </summary>
    public class HoleTrigger : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private Hole hole;

        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
            if (hole == null) {
                _logger.Warn("Hole is not set. Will attempt to automatically find hole.");
                hole = transform.parent.parent.GetComponent<Hole>();
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (GameManager.Instance.CurrentHole != hole) return;
            if (!other.CompareTag("Player")) return;
            _logger.Log("Player ("+other.name+") made it into the hole!");

            // TODO: Fix edge case where some clients can detect player as "in the hole" despite not being in the hole
            var player = other.GetComponent<PhotonView>().Owner;
            GameManager.Instance.PlayerInHole(player);
        }
    }
}
