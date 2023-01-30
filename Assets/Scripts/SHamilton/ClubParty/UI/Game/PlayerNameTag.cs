using Photon.Pun;
using TMPro;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.Game {
    /// <summary>
    /// Sets this text to be the player's name
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PlayerNameTag : MonoBehaviour {
    
        [SerializeField] private bool debug;

        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
            
            var view = GetComponentInParent<PhotonView>();
            if (view.IsMine) {
                _logger.Log("This is the LocalPlayer. This name tag will be destroyed.");
                Destroy(gameObject);
            }
            
            var text = GetComponent<TextMeshProUGUI>();
            text.text = view.Owner.NickName;
            _logger.Log("Text set to "+text.text);
        }
    }
}
