using System;
using SHamilton.ClubParty.Network;

namespace SHamilton.ClubParty.PowerUp {
    public abstract class PowerUpData {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Type ComponentType { get; }

        public abstract void Selected();

        protected void RemoveFromStorage() {
            var storedPowerUps = NetworkManager.LocalCharacter.GetComponent<StoredPowerUps>();
            storedPowerUps.Remove(this);
        }
    }
}