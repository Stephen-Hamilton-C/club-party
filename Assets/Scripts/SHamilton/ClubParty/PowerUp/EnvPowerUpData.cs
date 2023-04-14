using System;
using SHamilton.ClubParty.UI.Flair;

namespace SHamilton.ClubParty.PowerUp {
    public abstract class EnvPowerUpData : PowerUpData {
        public override SelectableColor.Colors ButtonColor => SelectableColor.Colors.Orange;

        public override void Selected() {
            // Show the environment UI
            throw new NotImplementedException();
        }
    }
}