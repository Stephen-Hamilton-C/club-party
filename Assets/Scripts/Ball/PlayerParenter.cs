using Network;
using Photon.Pun;
using UnityEngine;

namespace Ball {
    [RequireComponent(typeof(PhotonView))]
    public class PlayerParenter : MonoBehaviour {
    
        [SerializeField] private bool debug;

        private static Logger _logger;
        private PhotonView _view;

        public static Transform CharacterContainer {
            get {
                if (!_characterContainer) {
                    _logger.Log("Creating CharacterContainer...");
                    _characterContainer = new GameObject("CharacterContainer").transform;
                }

                return _characterContainer;
            }
        }
        private static Transform _characterContainer;
        
        private void Awake() {
            _logger ??= new(this, false);
            if(debug)
                _logger.Enabled = true;
            
            _view = GetComponent<PhotonView>();
            
            // Don't need to RPC this - every client will perform this calculation when the GameObject is created
            transform.parent = CharacterContainer;
            gameObject.name = _view.Owner.ActorNumber + " " + _view.Owner.NickName;

            if (_view.IsMine) {
                NetworkManager.SetPlayerProperty("CharacterName", gameObject.name);
            }
        }

    }
}

