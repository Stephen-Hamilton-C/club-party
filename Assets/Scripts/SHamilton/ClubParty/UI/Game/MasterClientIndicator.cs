using Photon.Pun;
using UnityEngine;

namespace SHamilton.ClubParty.UI.Game {
    /// <summary>
    /// Indicates if this player character belongs to the master client
    /// </summary>
    public class MasterClientIndicator : MonoBehaviour {

        private PhotonView _view;

        private void Start() {
            _view = GetComponentInParent<PhotonView>();
        }

        private void Update() {
            gameObject.SetActive(_view.Owner.IsMasterClient);
        }
    }
}
