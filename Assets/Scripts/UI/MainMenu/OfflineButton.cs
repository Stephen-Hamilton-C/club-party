using Network;

namespace UI.MainMenu {
    /// <summary>
    /// Initializes offline mode when clicked
    /// </summary>
    public class OfflineButton : ButtonBase {

        private Logger _logger;

        protected override void Start() {
            base.Start();
            _logger = new(this, debug);
        }

        protected override void OnClick() {
            _logger.Log("Enabling offline mode...");
            NetworkManager.onJoinedRoom += GameManager.StartGame;
            NetworkManager.ConnectOfflineAndJoinRoom();
        }

        private void OnDestroy() {
            NetworkManager.onJoinedRoom -= GameManager.StartGame;
        }
    }
}
