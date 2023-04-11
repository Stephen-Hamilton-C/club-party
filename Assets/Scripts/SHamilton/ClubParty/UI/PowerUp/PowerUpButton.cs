using System;
using SHamilton.ClubParty.Network;
using SHamilton.ClubParty.PowerUp;
using SHamilton.ClubParty.UI.Flair;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.PowerUp {
    [RequireComponent(typeof(SelectableColor))]
    public class PowerUpButton : ButtonBase {
    
        [SerializeField] private int powerUpIndex;

        private Logger _logger;
        private StoredPowerUps _storedPowerUps;
        private PowerUpData PowerUp => _storedPowerUps.PowerUps[powerUpIndex];
        private SelectableColor _buttonColor;
	
        protected override void Start() {
            base.Start();
            
            _logger = new(this, debug);
            _buttonColor = GetComponent<SelectableColor>();
            _storedPowerUps = NetworkManager.LocalCharacter.GetComponent<StoredPowerUps>();
            if (powerUpIndex >= _storedPowerUps.MaxPowerUps) {
                throw new InvalidOperationException("powerUpIndex is out of bounds!");
            }
        }

        private void Update() {
            var visible = _storedPowerUps.PowerUps.Count - 1 >= powerUpIndex;
            Button.enabled = visible;
            var color = ButtonImage.color;
            color.a = visible ? 1f : 0f;
            ButtonImage.color = color;
            if (visible) {
                Button.interactable = PowerUp.CanSelect;
                ButtonText!.text = PowerUp.Name;
                _buttonColor.Color = PowerUp.ButtonColor;
            } else {
                ButtonText!.text = "";
            }
        }

        protected override void OnClick() {
            PowerUp.Selected();
        }

    }
}

