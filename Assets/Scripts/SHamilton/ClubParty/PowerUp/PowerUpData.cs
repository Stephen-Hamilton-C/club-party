using System;
using SHamilton.ClubParty.Ball;
using SHamilton.ClubParty.Network;
using UnityEngine;

namespace SHamilton.ClubParty.PowerUp {
    public abstract class PowerUpData {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Type ComponentType { get; }
        public abstract Color BackgroundColor { get; }
        public abstract Color ForegroundColor { get; }
        public virtual bool CanSelect => LocalPlayerState.CanStroke;

        public abstract void Selected();

        protected void RemoveFromStorage() {
            var storedPowerUps = NetworkManager.LocalCharacter.GetComponent<StoredPowerUps>();
            storedPowerUps.Remove(this);
        }
    }
}