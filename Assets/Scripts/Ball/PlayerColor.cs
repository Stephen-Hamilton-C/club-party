using Photon.Pun;
using UnityEngine;

namespace Ball {
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

            var color = (Color) _view.Owner.CustomProperties["CharacterColor"];
            _logger.Log("Changing color to "+color);
            _renderer.material.color = color;
        }
    }
}
