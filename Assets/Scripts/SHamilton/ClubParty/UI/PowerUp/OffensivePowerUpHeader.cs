using SHamilton.Util;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.PowerUp {
    public class OffensivePowerUpHeader : TextBase {
    
        private Logger _logger;
        private PlaceholderReplacer _replacer;

        protected override void Start() {
            base.Start();
            
            if(_logger == null)
                _logger = new(this, debug);
            
            _logger.Log("Current power up: "+OffensivePowerUpSelector.CurrentPowerUp!.Name);
            _replacer = new PlaceholderReplacer(Text.text)
                .Replace("POWERUP").With(OffensivePowerUpSelector.CurrentPowerUp!.Name);

            Text.text = _replacer.ReplacePlaceholders();
        }

    }
}

