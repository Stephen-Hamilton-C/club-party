using SHamilton.ClubParty.PowerUp;
using SHamilton.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI.Game {
    public class AppliedPowerUpInfo : MonoBehaviour {
        public PowerUpComponent powerUp;
        
        [SerializeField] private Sprite selfPowerUpSprite;
        [SerializeField] private Sprite offensivePowerUpSprite;
        [SerializeField] private Sprite envPowerUpSprite;
        
        private TMP_Text _text;
        private Image _image;
        private Placeholder _replacer;

        private void Start() {
            _text = GetComponentInChildren<TMP_Text>();
            _image = GetComponent<Image>();
            _replacer = new Placeholder(_text.text);
        }

        private void Update() {
            if (!powerUp) return;

            _text.text = _replacer
                .Set("NAME", powerUp.Data.Name)
                .Set("N", powerUp.Amount)
                .Replace();

            _image.sprite = powerUp.Data switch {
                SelfPowerUpData => selfPowerUpSprite,
                OffensivePowerUpData => offensivePowerUpSprite,
                EnvPowerUpData => envPowerUpSprite,
                _ => _image.sprite,
            };
        }
    }
}
