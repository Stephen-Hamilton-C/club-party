using System;
using Photon.Pun;
using UnityEngine;
using WebSocketSharp;

namespace Ball.PowerUps {
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PowerUpManager))]
    public abstract class PowerUp : MonoBehaviour {

        public abstract string PowerUpName { get; protected set; }
        public abstract string PowerUpDescription { get; protected set; }

        protected PlayerController Controller;
        protected PhotonView View;

        private PowerUpManager _manager;
        private Logger _logger;

        protected virtual void Awake() {
            _logger = new(this, Application.isEditor);
            
            View = GetComponent<PhotonView>();
            if (!View.IsMine) {
                Destroy(this);
            }

            _manager = GetComponent<PowerUpManager>();
            Controller = GetComponent<PlayerController>();
            PlayerState.OnStroke += Stroked;
        }

        protected virtual void Stroked() {
            _logger.Log("Stroked was not overridden for "+PowerUpName);
        }

        private void OnDestroy() {
            _manager.RemovePowerUp(GetType());
            PlayerState.OnStroke -= Stroked;
        }
    }
}
