using UnityEngine;
using UnityEngine.UI;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI {
    public class Contrast : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private Color onDarkBackground = Color.white;
        [SerializeField] private Color onBrightBackground = Color.black;
        [SerializeField, Range(0f, 1f)] private float contrast = 0.5f;

        protected Color ForegroundColor {
            get {
                #if UNITY_EDITOR
                _background = transform.parent.GetComponent<Image>() ?? GetComponentInParent<Image>();
                #endif
                Color.RGBToHSV(_background.color, out _, out _, out var brightness);
                return brightness >= contrast ? onBrightBackground : onDarkBackground;
            }
        }

        private Logger _logger;
        private Image _background;
	
        protected virtual void Start() {
            _logger = new(this, debug);
            _background = transform.parent.GetComponent<Image>() ?? GetComponentInParent<Image>();
        }
    }
}
