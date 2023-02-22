using System;
using JetBrains.Annotations;
using SHamilton.ClubParty.PowerUp;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.PowerUp {
    public class OffensivePowerUpSelector : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private GameObject panel;

        public OffensivePowerUpSelector Instance => _instance;
        private OffensivePowerUpSelector _instance;
        [CanBeNull] public OffensivePowerUpData CurrentPowerUp => _currentPowerUp;
        [CanBeNull] private OffensivePowerUpData _currentPowerUp = null;

        private Logger _logger;

        private void Awake() {
            _logger = new(this, debug);
            if (_instance != null) {
                _logger.Err("Another instance of OffensivePowerUpSelector exists! This duplicate instance will be destroyed.");
                Destroy(this);
            } else {
                _instance = this;
            }
        }

        public void OffensivePowerUpSelected(OffensivePowerUpData powerUp) {
            _currentPowerUp = powerUp;
            panel.SetActive(true);
        }
    }
}

