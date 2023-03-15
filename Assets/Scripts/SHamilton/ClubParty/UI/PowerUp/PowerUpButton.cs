using System;
using SHamilton.ClubParty.Network;
using SHamilton.ClubParty.PowerUp;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.PowerUp {
    public class PowerUpButton : ButtonBase {
    
        [SerializeField] private int powerUpIndex;

        private Logger _logger;
        private StoredPowerUps _storedPowerUps;
        private PowerUpData PowerUp => _storedPowerUps.PowerUps[powerUpIndex];
	
        protected override void Start() {
            base.Start();
            
            _logger = new(this, debug);
            _storedPowerUps = NetworkManager.LocalCharacter.GetComponent<StoredPowerUps>();
            if (powerUpIndex >= _storedPowerUps.MaxPowerUps) {
                throw new InvalidOperationException("powerUpIndex is out of bounds!");
            }
        }

        private void Update() {
            var visible = _storedPowerUps.PowerUps.Count - 1 >= powerUpIndex;
            Button.enabled = visible;
            ButtonImage.enabled = visible;
            if (visible) {
                Button.interactable = PowerUp.CanSelect;
                ButtonText!.text = PowerUp.Name;
                Button.image.color = PowerUp.BackgroundColor;
                ButtonText.color = PowerUp.ForegroundColor;
            } else {
                ButtonText!.text = "";
            }
        }

        protected override void OnClick() {
            PowerUp.Selected();
        }

    }
}

