using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.Settings {
    public class SliderSetting : SliderBase {

        [SerializeField] private string settingKey;
        [SerializeField] private float defaultValue;
        
        private Logger _logger;
	
        protected override void Start() {
            base.Start();
            _logger = new(this, debug);
            if (string.IsNullOrEmpty(settingKey)) {
                _logger.Err("settingKey is not filled out in inspector! This script will destroy itself ("+gameObject.name+")");
                Destroy(this);
            }

            Slider.value = Slider.wholeNumbers
                ? PlayerPrefs.GetInt(settingKey, Mathf.RoundToInt(defaultValue))
                : PlayerPrefs.GetFloat(settingKey, defaultValue);
            _logger.Log("Initialized value to "+Slider.value);
        }

        protected override void OnValueChanged(float value) {
            if (Slider.wholeNumbers) {
                var intValue = Mathf.RoundToInt(value);
                PlayerPrefs.SetInt(settingKey, intValue);
                _logger.Log("Set value to "+intValue);
            } else {
                PlayerPrefs.SetFloat(settingKey, value);
                _logger.Log("Set value to "+value);
            }
        }
    }
}
