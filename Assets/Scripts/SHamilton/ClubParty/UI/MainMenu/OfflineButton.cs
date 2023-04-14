using Photon.Realtime;
using SHamilton.ClubParty.Network;
using SHamilton.Util;

namespace SHamilton.ClubParty.UI.MainMenu {
    /// <summary>
    /// Initializes offline mode when clicked
    /// </summary>
    public class OfflineButton : ButtonBase {

        private Logger _logger;

        protected override void Start() {
            base.Start();
            _logger = new(this, debug);
            NetworkManager.onConnectionStart += ConnectionStarted;
            NetworkManager.onDisconnected += ConnectionFailed;
        }

        protected override void OnClick() {
            _logger.Log("Enabling offline mode...");
            NetworkManager.onJoinedRoom += GameManager.StartGame;
            NetworkManager.ConnectOfflineAndJoinRoom();
        }

        private void ConnectionStarted() {
            _logger.Log("Connection started. Disabling button.");
            Button.interactable = false;
        }

        private void ConnectionFailed(DisconnectCause cause) {
            Button.interactable = true;
        }


        private void OnDestroy() {
            NetworkManager.onJoinedRoom -= GameManager.StartGame;
            NetworkManager.onConnectionStart -= ConnectionStarted;
            NetworkManager.onDisconnected -= ConnectionFailed;
        }
    }
}
