using System;
using UnityEngine;

namespace UI {
    public class OfflineButton : ButtonBase {

        private Logger _logger;

        protected override void Start() {
            base.Start();
            _logger = new(this, debug);
        }

        protected override void OnClick() {
            _logger.Log("Enabling offline mode...");

            NetworkManager.onConnectedToMaster += ConnectedToMaster;
            NetworkManager.onJoinedRoom += GameManager.StartGame;
            NetworkManager.ConnectOffline();
        }

        private void ConnectedToMaster() {
            _logger.Log("Joining room...");
            NetworkManager.JoinRoom();
        }

        private void OnDestroy() {
            NetworkManager.onConnectedToMaster -= ConnectedToMaster;
            NetworkManager.onJoinedRoom -= GameManager.StartGame;
        }
    }
}
