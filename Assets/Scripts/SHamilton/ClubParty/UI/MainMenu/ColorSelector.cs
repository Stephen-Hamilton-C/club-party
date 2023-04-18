using SHamilton.ClubParty.Network;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.MainMenu {
    [RequireComponent(typeof(ToggleGroup))]
    public class ColorSelector : MonoBehaviour {
    
        [SerializeField] private bool debug;

        public UnityEvent<Color> onColorSelected = new();

        private Logger _logger;
        private ToggleGroup _group;
        private Toggle[] _toggles;
	
        private void Start() {
            _logger = new(this, debug);
            _group = GetComponent<ToggleGroup>();
            _toggles = GetComponentsInChildren<Toggle>();

            var selectedColor = Prefs.CharacterColor;
            foreach (var toggle in _toggles) {
                toggle.isOn = toggle.image.color == selectedColor;
                
                _logger.Log("Adding "+toggle.name);
                toggle.onValueChanged.AddListener((value) => {
                    _logger.Log(toggle.name+" changed to "+value);
                    if (value)
                        UpdateColor(toggle);
                });

                if (toggle.isOn)
                    UpdateColor(toggle);
            }
            
            _group.EnsureValidState();
        }

        private void UpdateColor(Toggle toggle) {
            var color = toggle.image.color;
            Prefs.CharacterColor = color;
            NetworkManager.LocalPlayer.SetCharacterColor(color);
            onColorSelected.Invoke(color);
            _logger.Log("Saved color as "+toggle.name+" and set CharacterColor to "+color);
        }
    }
}

