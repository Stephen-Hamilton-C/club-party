using Network;
using Photon.Pun;

namespace UI.Game {
    public class DisconnectButton : ButtonBase {
    
        private Logger _logger;
	
        protected override void Start() {
            base.Start();
            _logger = new(this, debug);
        }

        protected override void OnClick() {
            PhotonNetwork.Disconnect();
        }
    }
}
