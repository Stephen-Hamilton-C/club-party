using SHamilton.ClubParty.Ball;
using SHamilton.Util;

namespace SHamilton.ClubParty.UI.Game {
    public class MenuButton : ButtonBase {
    
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
