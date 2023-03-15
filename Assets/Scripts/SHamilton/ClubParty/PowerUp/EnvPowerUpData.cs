using UnityEngine;
using System;

namespace SHamilton.ClubParty.PowerUp {
    public abstract class EnvPowerUpData : PowerUpData {
        public override Color BackgroundColor => Color.yellow;
        public override Color ForegroundColor => Color.black;

        public override void Selected() {
            // Show the environment UI
            throw new NotImplementedException();
        }
    }
}