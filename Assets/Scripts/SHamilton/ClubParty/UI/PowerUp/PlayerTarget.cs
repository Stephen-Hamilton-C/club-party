using System;
using Photon.Realtime;
using SHamilton.ClubParty.Network;
using SHamilton.ClubParty.PowerUp;
using SHamilton.Util;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.PowerUp {
    public class PlayerTarget : ButtonBase {
    
        public Player Player;
        
        private Logger _logger;
        private Placeholder _replacer;
        private OffensivePowerUpData PowerUp => OffensivePowerUpSelector.CurrentPowerUp;
        private GameObject _character;
	
        protected override void Start() {
            base.Start();
            
            _logger = new(this, debug);
            _replacer = new Placeholder(ButtonText!.text)
                .Set("PLAYERNAME", Player.NickName);
            ButtonText!.text = _replacer.Replace();

            _character = Player.GetCharacter();
        }

        private void Update() {
            // Don't let player be selected if the PowerUp is already applied
            // Button.interactable = !_character.TryGetComponent(PowerUp.ComponentType, out _);
        }

        protected override void OnClick() {
            OffensivePowerUpSelector.Instance.PlayerSelected(Player);
        }
    }
}

