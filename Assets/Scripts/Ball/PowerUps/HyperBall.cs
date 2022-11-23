using Photon.Pun;
using UnityEngine;

namespace Ball.PowerUps {
    public class HyperBall : PowerUp {

        private const float SpeedFactor = 25;
        
        public override string PowerUpName { get; protected set; } = "HyperBall";
        public override string PowerUpDescription { get; protected set; } = "Go really, really fast on your next stroke";

        private float _oldSpeed;
        private PlayerController _controller;
        
        protected override void OnLocalPlayerEntered() {
            // Make it appear as though the power-up was picked up
            GetComponent<Renderer>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;

            if (Manager.AddPowerUp(this)) {
                // Power-up not applied, apply effect
                _controller = LocalCharacter.GetComponent<PlayerController>();
                _oldSpeed = _controller.speed;
                _controller.speed *= SpeedFactor;
            } else {
                // Already applied, remove
                PhotonNetwork.Destroy(gameObject);
            }
        }

        protected override void Stroked() {
            if (Triggered) {
                _controller.speed = _oldSpeed;
                Manager.RemovePowerUp(this);
                PhotonNetwork.Destroy(gameObject);
            }
        }
        
    }
}
