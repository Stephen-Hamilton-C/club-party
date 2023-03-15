using System;

namespace SHamilton.ClubParty.PowerUp.HoleMagnet {
    public class HoleMagnetData : SelfPowerUpData {
        public override string Name => "Hole Magnet";
        public override string Description => "Your ball really wants to get in the hole.";
        public override Type ComponentType => typeof(HoleMagnetPowerUp);
    }
}

