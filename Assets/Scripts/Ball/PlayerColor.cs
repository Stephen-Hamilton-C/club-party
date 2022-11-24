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

            var colorData = _view.Owner.CustomProperties["CharacterColor"] as string;
            var colorSplit = colorData!.Split(",");
            var color = new Color(float.Parse(colorSplit[0]), float.Parse(colorSplit[1]), float.Parse(colorSplit[2]));
            _logger.Log("Changing color to "+color);
            _renderer.material.color = color;
        }
    }
}
