using SHamilton.ClubParty.Ball;
using UnityEngine;

namespace SHamilton.ClubParty.PowerUp.Hyperball {
    [RequireComponent(typeof(PlayerController))]
    public class HyperballPowerUp : PowerUpComponent {

        private const float SpeedFactor = 10f;
        
        private PlayerController _controller;
        private float _origSpeed;
	
        protected override void Start() {
            base.Start();
            _controller = GetComponent<PlayerController>();

            if (View.IsMine) {
                // Apply Hyperball
                _origSpeed = _controller.speed;
                _controller.speed *= SpeedFactor;
                LocalPlayerState.OnStroke += Stroked;
            }
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            
            // Reset state
            _controller.speed = _origSpeed;
            LocalPlayerState.OnStroke -= Stroked;
        }

        private void Stroked() {
            Amount--;
        }
    }
}

