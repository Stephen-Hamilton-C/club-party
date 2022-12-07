using Photon.Pun;
using UnityEngine;

namespace Ball.PowerUps {
    [RequireComponent(typeof(PhotonView))]
    public abstract class PlayerPowerUp : PowerUp {

        /// <summary>
        /// Can be applied to other players?
        /// </summary>
        public abstract bool Offensive { get; }
        
        private PowerUpManager _powerUpMan;
	
        protected override void Start() {
            _powerUpMan = GetComponent<PowerUpManager>();
            base.Start();
        }

        protected override void OnDestroy() {
            _powerUpMan.PowerUpFinished(this.GetType());
            base.OnDestroy();
        }
    }
}

