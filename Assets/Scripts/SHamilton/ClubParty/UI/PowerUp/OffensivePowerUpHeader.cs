using SHamilton.Util;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.PowerUp {
    public class OffensivePowerUpHeader : TextBase {
    
        private Logger _logger;
        private PlaceholderReplacer _replacer;
	
        private void Start() {
            _logger = new(this, debug);
            _replacer = new(Text.text);
        }
    }
}

