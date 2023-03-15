using Photon.Pun;
using SHamilton.ClubParty.Network;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.Ball {
    /// <summary>
    /// Handles placing the character into the CharacterContainer
    /// so that the NetworkManager can find the player characters
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class PlayerSetup : MonoBehaviour {
    
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
            // Only initialize logger once
            _logger ??= new(this, debug);
            _view = GetComponent<PhotonView>();
            
            // Don't need to RPC this - every client will perform this calculation when the player character is created
            transform.parent = CharacterContainer;
            gameObject.name = _view.Owner.ActorNumber + " " + _view.Owner.NickName;
            //new PlayerProperties(_view.Owner).Character = gameObject;
            _view.Owner.SetCharacter(gameObject);
        }

    }
}

