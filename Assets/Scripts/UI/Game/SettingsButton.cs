using System;
using Ball;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Game {
    public class SettingsButton : ButtonBase {
    
        private Logger _logger;
        private bool _clicked;

        protected override void Start() {
            base.Start();
            _logger = new(this, debug);
            
        }

        protected override void OnClick() {
            _clicked = true;
            PlayerController.Deactivate();
        }

        private void Update() {
            if (_clicked && gameObject.activeSelf) {
                // Came back from settings
                _clicked = false;
                PlayerController.Activate();
            }
        }
    }
}
