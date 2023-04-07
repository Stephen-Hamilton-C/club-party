using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.MainMenu {
    [RequireComponent(typeof(Renderer))]
    public class BallColor : MonoBehaviour {
    
        [SerializeField] private bool debug;

        private Logger _logger;
        private Renderer _renderer;
	
        private void Start() {
            _logger = new(this, debug);
            _renderer = GetComponent<Renderer>();
        }

        public void ColorChanged(Color color) {
            _logger.Log("Set preview ball color to "+color);
            _renderer.material.color = color;
        }
    }
}

