using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ColorSelector : MonoBehaviour {
    
        [SerializeField] private bool debug;

        private Logger _logger;
        private ToggleGroup _group;

        private void Start() {
            _logger = new(this, debug);
            _group = GetComponent<ToggleGroup>();

            var colorName = PlayerPrefs.GetString("CharacterColor", "Azure");
            Toggle[] toggles = GetComponentsInChildren<Toggle>();
            foreach (var toggle in toggles) {
                toggle.isOn = toggle.name == colorName;
                toggle.onValueChanged.AddListener((bool value) => { ToggleChanged(toggle, value); });
                
                if (toggle.isOn)
                    UpdateColor(toggle);
            }
        }

        private void ToggleChanged(Toggle toggle, bool value) {
            if (value)
                UpdateColor(toggle);
        }

        private void UpdateColor(Toggle toggle) {
            PlayerPrefs.SetString("CharacterColor", toggle.name);
            var color = toggle.GetComponent<Image>().color;
            var colorData = color.r + "," + color.g + "," + color.b;
            PhotonNetwork.LocalPlayer.CustomProperties["CharacterColor"] = colorData;
        }

    }
}
