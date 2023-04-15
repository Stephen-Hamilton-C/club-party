using SHamilton.Util;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.PowerUp {
    public class OffensivePowerUpHeader : TextBase {
    
        private Logger _logger;
        private Placeholder _replacer;

        protected override void Start() {
            base.Start();
            
            if(_logger == null)
                _logger = new(this, debug);
            
            _logger.Log("Current power up: "+OffensivePowerUpSelector.CurrentPowerUp!.Name);
            _replacer = new Placeholder(Text.text)
                .Set("POWERUP", OffensivePowerUpSelector.CurrentPowerUp!.Name);

            Text.text = _replacer.Replace();
        }

    }
}

