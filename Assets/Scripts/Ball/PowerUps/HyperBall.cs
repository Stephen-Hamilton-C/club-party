using JetBrains.Annotations;
using Photon.Pun;

namespace Ball.PowerUps {
    /// <summary>
    /// Ball go very fast. Intended to be applied to another player when picked up
    /// </summary>
    public class HyperBall : PowerUp {

        /// <summary>
        /// How much the ball will zoom when applied
        /// </summary>
        private const float SpeedFactor = 10;
        
        public override string PowerUpName { get; protected set; } = "HyperBall";
        public override string PowerUpDescription { get; protected set; } = "Go really, really fast on your next stroke";

        /// <summary>
        /// The original speed of the player when this effect was applied
        /// </summary>
        private float _oldSpeed;
        /// <summary>
        /// The controller of the player affected
        /// </summary>
        private PlayerController _controller;
        
        protected override void OnLocalPlayerTouched() {
            if (Manager.AddPowerUp(this)) {
                // Power-up not already applied, apply effect
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
                // Power up has taken effect, remove power up
                _controller.speed = _oldSpeed;
                Manager.RemovePowerUp(this);
                PhotonNetwork.Destroy(gameObject);
            }
        }

        /// <summary>
        /// This is implemented in the base class, but because of how RPCs work, this needs to be here as well
        /// </summary>
        [PunRPC]
        [UsedImplicitly]
        protected override void HideRPC() {
            base.HideRPC();
        }
    }
}
