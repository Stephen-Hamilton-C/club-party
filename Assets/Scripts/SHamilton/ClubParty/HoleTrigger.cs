using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty {
    /// <summary>
    /// Listens for players who have touched this hole, then informs the GameManager
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class HoleTrigger : MonoBehaviour {
    
        [SerializeField] private bool debug;
        public Hole hole;

        private Logger _logger;
        private PhotonView _view;
	
        private void Start() {
            _logger = new(this, debug);
            _view = GetComponent<PhotonView>();
            
            if (hole == null) {
                _logger.Warn("Hole is not set. Will attempt to automatically find hole.");
                hole = transform.parent.parent.GetComponent<Hole>();
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (!hole.isCurrent) return;
            if (!other.CompareTag("Player")) return;
            var view = other.GetComponent<PhotonView>();
            if (!view.IsMine) return;
            _logger.Log("LocalPlayer made it into the hole!");

            _view.RPC("PlayerInHoleRPC", RpcTarget.AllBuffered, view.Owner);
        }

        [PunRPC, UsedImplicitly]
        private void PlayerInHoleRPC(Player player) {
            if (!hole.isCurrent) return;
            _logger.Log("Player "+player+" has made it into the hole.");
            GameManager.Instance.PlayerInHole(player);
        }
    }
}
