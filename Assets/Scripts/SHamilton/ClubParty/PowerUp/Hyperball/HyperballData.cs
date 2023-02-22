using System;

namespace SHamilton.ClubParty.PowerUp.Hyperball {
    public class HyperballData : OffensivePowerUpData {
        public override string Name => "Hyperball";
        public override string Description => "Make an opponent go fast. I mean really, really fast.";
        public override Type ComponentType { get; } = typeof(HyperballPowerUp);
    }
}