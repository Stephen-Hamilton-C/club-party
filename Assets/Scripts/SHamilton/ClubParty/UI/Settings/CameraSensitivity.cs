using UnityEngine;
using UnityEngine.UI;

// TODO: Make a more general class for this
namespace SHamilton.ClubParty.UI.Settings {
    [RequireComponent(typeof(Slider))]
    public class CameraSensitivity : MonoBehaviour {
        
        private void Start() {
            var slider = GetComponent<Slider>();
            slider.value = PlayerPrefs.GetFloat("MouseSensitivity", slider.value);
            slider.onValueChanged.AddListener((value) => {
                PlayerPrefs.SetFloat("MouseSensitivity", value);
            });
        }
    }
}

