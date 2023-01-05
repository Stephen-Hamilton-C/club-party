using Network;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu {
    /// <summary>
    /// Handles applying selected color
    /// </summary>
    public class ColorSelector : MonoBehaviour {
    
        [SerializeField] private bool debug;

        private Logger _logger;

        private void Start() {
            _logger = new(this, debug);

            var colorName = PlayerPrefs.GetString("CharacterColor", "Azure");
            var toggles = GetComponentsInChildren<Toggle>();
            foreach (var toggle in toggles) {
                // Ensure the correct toggle is activated
                toggle.isOn = toggle.name == colorName;
                
                // Update the CharacterColor when a toggle changes
                toggle.onValueChanged.AddListener((bool value) => {
                    if(value)
                        UpdateColor(toggle);
                });
                
                // Update the CharacterColor with the active toggle
                if (toggle.isOn)
                    UpdateColor(toggle);
            }
        }

        private void UpdateColor(Toggle toggle) {
            PlayerPrefs.SetString("CharacterColor", toggle.name);
            var color = toggle.GetComponent<Image>().color;
            NetworkManager.LocalPlayerProperties.CharacterColor = color;
            _logger.Log("Saved color as "+toggle.name+" and set CharacterColor to "+color);
        }

    }
}
