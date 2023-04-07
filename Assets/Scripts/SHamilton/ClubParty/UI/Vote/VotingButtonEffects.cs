using UnityEngine;
using UnityEngine.UI;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.Vote {
    [RequireComponent(typeof(Toggle))]
    [RequireComponent(typeof(Image))]
    public class VotingButtonEffects : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private Color selectedColor;
        [SerializeField] private float transitionTime = 0.25f;

        private Logger _logger;
        private Toggle _toggle;
        private Image _image;
        private Color _deselectedColor;
	
        private void Start() {
            _logger = new(this, debug);
            _toggle = GetComponent<Toggle>();
            _image = GetComponent<Image>();

            _deselectedColor = _image.color;
            _toggle.onValueChanged.AddListener(ValueChanged);
        }

        private void ValueChanged(bool value) {
            var color = value ? selectedColor : _deselectedColor;
            _image.color = color;
            _logger.Log("Color set to "+color);
        }
    }
}

