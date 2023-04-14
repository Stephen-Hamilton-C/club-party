using Photon.Realtime;
using SHamilton.ClubParty.Network;
using UnityEngine;
using UnityEngine.UI;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.MainMenu {
    /// <summary>
    /// Initializes a connection with Photon when clicked
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class MultiplayerButton : ButtonBase {

        private Logger _logger;
        private string _origText;

        protected override void Start() {
            base.Start();
            _logger = new(this, debug);
            _origText = ButtonText!.text;
            
            NetworkManager.onConnectionStart += ConnectionStarted;
            NetworkManager.onDisconnected += ConnectionFailed;
        }

        protected override void OnClick() {
            var willConnect = NetworkManager.ConnectAndJoinRoom();
            _logger.Log("Connecting to Master Server...");
            _logger.Log(willConnect ? "Will connect..." : "Won't connect.");
            
            if (willConnect) {
                NetworkManager.onJoinedRoom += GameManager.StartGame;
                ButtonText!.text = "Connecting...";
            }
        }

        private void ConnectionStarted() {
            _logger.Log("Connection started. Disabling button.");
            Button.interactable = false;
        }

        private void ConnectionFailed(DisconnectCause cause) {
            ButtonText!.text = _origText;
            Button.interactable = true;
            new DialogBuilder()
                .SetTitle("Connection Error")
                .SetContent("There was a problem connecting to Photon servers. Are you online? Error cause: " + cause)
                .Build();
        }

        private void OnDestroy() {
            NetworkManager.onJoinedRoom -= GameManager.StartGame;
            NetworkManager.onConnectionStart -= ConnectionStarted;
            NetworkManager.onDisconnected -= ConnectionFailed;
        }
    }
}