using UnityEngine;
using UnityEngine.UI;

// TODO: Make a more general class for this
namespace SHamilton.ClubParty.UI.Settings {
    [RequireComponent(typeof(Slider))]
    public class CameraSensitivity : MonoBehaviour {
        
        private void Start() {
            var slider = GetComponent<Slider>();
            slider.value = Prefs.MouseSensitivity;
            slider.onValueChanged.AddListener((value) => {
                Prefs.MouseSensitivity = value;
            });
        }
    }
}

