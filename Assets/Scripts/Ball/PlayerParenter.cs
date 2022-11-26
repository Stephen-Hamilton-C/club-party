using JetBrains.Annotations;
using Network;
using Photon.Pun;
using UnityEngine;

namespace Ball {
    /// <summary>
    /// Handles placing the character into the CharacterContainer
    /// so that the NetworkManager can find the player characters
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class PlayerParenter : MonoBehaviour {
    
        [SerializeField] private bool debug;

        private static Logger _logger;
        private PhotonView _view;

        /// <summary>
        /// The GameObject that contains all characters
        /// </summary>
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
            // TODO: Did I make this overly complicated???? Can't I just _view.Owner["Character"] = gameObject????????
            // Lol it's 12 am, I think my brain fried
            
            // Only initialize the Logger once
            _logger ??= new(this, false);
            // If any instance has debug set to true, then by golly enable the logger
            if(debug)
                _logger.Enabled = true;
            
            _view = GetComponent<PhotonView>();
            
            // Don't need to RPC this - every client will perform this calculation when the GameObject is created
            transform.parent = CharacterContainer;
            gameObject.name = _view.Owner.ActorNumber + " " + _view.Owner.NickName;

            // Set the CustomProperty 
            if (_view.IsMine) {
                NetworkManager.SetPlayerProperty("CharacterName", gameObject.name);
            }
        }

        /// <summary>
        /// Sets the Character reference locally
        /// </summary>
        [PunRPC]
        [UsedImplicitly]
        private void SetCharacterRefRPC() {
            _view.Owner.CustomProperties["Character"] = gameObject;
        }

    }
}

