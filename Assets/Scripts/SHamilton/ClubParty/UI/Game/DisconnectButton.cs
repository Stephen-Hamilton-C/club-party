using SHamilton.ClubParty.Network;
using SHamilton.Util;

namespace SHamilton.ClubParty.UI.Game {
    public class DisconnectButton : ButtonBase {
    
        private Logger _logger;
	
        protected override void Start() {
            base.Start();
            _logger = new(this, debug);
        }

        protected override void OnClick() {
            NetworkManager.Disconnect();
        }
    }
}
