using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Button))]
    public class MultiplayerButton : ButtonBase {

        private Logger _logger;

        protected override void Start() {
            base.Start();
            _logger = new(this, debug);
        }

        protected override void OnClick() {
            bool willConnect = NetworkManager.ConnectAndJoinRoom();
            _logger.Log("Connecting to Master Server...");
            _logger.Log(willConnect ? "Will connect..." : "Won't connect.");
            
            if (willConnect) {
                // TODO: Need to find a way to communicate to UI that we're connecting
                // That way, other buttons and stuff can disable/have some other interaction or something
                NetworkManager.onJoinedRoom += GameManager.StartGame;
                Button.interactable = false;
                ButtonText.text = "Connecting...";
            }
        }

        private void OnDestroy() {
            NetworkManager.onJoinedRoom -= GameManager.StartGame;
        }
    }
}