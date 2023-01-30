using System;

namespace SHamilton.ClubParty.PowerUp.Hyperball {
    public class HyperballData : OffensivePowerUpData {
        public override string Name { get; } = "Hyperball";
        public override string Description { get; } = "Make an opponent go fast. I mean really, really fast.";
        public override Type ComponentType { get; } = typeof(HyperballPowerUp);
    }
}