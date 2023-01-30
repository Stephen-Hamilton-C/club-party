using Photon.Pun;
using SHamilton.ClubParty.Network;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.Ball {
    /// <summary>
    /// Sets the player's color
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(Renderer))]
    public class PlayerColor : MonoBehaviour {
    
        [SerializeField] private bool debug;

        private Logger _logger;
        private PhotonView _view;
        private Renderer _renderer;
	
        private void Start() {
            _logger = new(this, debug);
            _view = GetComponent<PhotonView>();
            _renderer = GetComponent<Renderer>();

            var color = new PlayerProperties(_view.Owner).CharacterColor;
            _logger.Log("Changing color to "+color);
            _renderer.material.color = color;
        }
    }
}
