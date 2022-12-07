using System;
using UnityEngine;

namespace Ball.PowerUps {
    public abstract class PowerUp : MonoBehaviour {
    
        public abstract string PowerUpName { get; }
        public abstract string PowerUpDescription { get; }

        protected virtual void Start() {
            ApplyEffect();
        }

        protected abstract void ApplyEffect();
        protected abstract void RemoveEffect();

        protected virtual void OnDestroy() {
            RemoveEffect();
        }
    }
}

