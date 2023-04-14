using TMPro;
using UnityEngine;

namespace SHamilton.ClubParty.UI.Game {
    [RequireComponent(typeof(TMP_Text))]
    public class ParText : MonoBehaviour {
        private TMP_Text _text;

        private void Start() {
            _text = GetComponent<TMP_Text>();

            GameManager.OnHoleFinished += UpdateText;
            UpdateText();
        }

        private void OnDestroy() {
            GameManager.OnHoleFinished -= UpdateText;
        }

        private void UpdateText() {
            var par = GameManager.Instance.CurrentHole.Par;
            _text.text = "Par: " + par;
        }
    }
}
