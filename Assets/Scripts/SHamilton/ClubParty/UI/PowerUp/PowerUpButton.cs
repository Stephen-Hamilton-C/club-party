using System;
using SHamilton.ClubParty.Network;
using SHamilton.ClubParty.PowerUp;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.PowerUp {
    public class PowerUpButton : ButtonBase {
    
        [SerializeField] private int powerUpIndex;

        private Logger _logger;
        private StoredPowerUps _powerUps;
        private PowerUpData PowerUp => _powerUps.PowerUps[powerUpIndex];
	
        protected override void Start() {
            base.Start();
            
            _logger = new(this, debug);
            _powerUps = NetworkManager.LocalCharacter.GetComponent<StoredPowerUps>();
            if (powerUpIndex >= _powerUps.MaxPowerUps) {
                throw new InvalidOperationException("powerUpIndex is out of bounds!");
            }
        }

        private void Update() {
            var visible = _powerUps.PowerUps.Count - 1 >= powerUpIndex;
            Button.enabled = visible;
            ButtonImage.enabled = visible;
            if (visible) {
                ButtonText!.text = PowerUp.Name;
            } else {
                ButtonText!.text = "";
            }
        }

        protected override void OnClick() {
            PowerUp.Selected();
        }

    }
}

