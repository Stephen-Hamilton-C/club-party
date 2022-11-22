using System;
using Photon.Pun;
using UnityEngine;

namespace Ball.PowerUps {
    public class HyperBall : PowerUp {

        private const float SpeedFactor = 25;
        
        public override string PowerUpName { get; protected set; } = "HyperBall";
        public override string PowerUpDescription { get; protected set; } = "Go really, really fast on your next stroke";

        private PlayerController _controller;
        private float _oldSpeed;
        
        protected override void Awake() {
            base.Awake();
            _controller = GetComponent<PlayerController>();
        }

        private void OnEnable() {
            _oldSpeed = _controller.speed;
            _controller.speed *= SpeedFactor;
        }

        protected override void Stroked() {
            Debug.Log("===================STROKED!");
            Destroy(this);
        }

        private void OnDisable() {
            _controller.speed = _oldSpeed;
        }
    }
}
