using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = SHamilton.Util.Logger;

// TODO: Make a new SliderBase prefab
namespace SHamilton.ClubParty.UI {
    [ExecuteInEditMode]
    [RequireComponent(typeof(TMP_Text))]
    public class SliderDisplay : MonoBehaviour {
    
        [SerializeField] private bool debug;

        private Logger _logger;
        private TMP_Text _text;
	
        private void Start() {
            _logger = new(this, debug);
            _text = GetComponent<TMP_Text>();
            
            var slider = transform.parent.parent.parent.GetComponent<Slider>();
            slider.onValueChanged.AddListener(ValueChanged);
            ValueChanged(slider.value);
        }

        private void ValueChanged(float value) {
            _text.text = (Mathf.Round(value * 100) / 100).ToString(CultureInfo.CurrentCulture);
        }
    }
}

