using System.Collections.Generic;
using SHamilton.ClubParty.Network;
using SHamilton.ClubParty.PowerUp;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.Game {
    public class AppliedPowerUps : MonoBehaviour {
        [SerializeField] private bool debug;
        [SerializeField] private GameObject infoTemplate;

        private StoredPowerUps _powerUps;
        private readonly Dictionary<PowerUpComponent, GameObject> _infos = new();
        private Logger _logger;

        private void Start() {
            _logger = new(this, debug);
            _powerUps = NetworkManager.LocalCharacter.GetComponent<StoredPowerUps>();
            _powerUps.OnPowerUpApplied += PowerUpApplied;
            _powerUps.OnAppliedPowerUpRemoved += PowerUpRemoved;
        }

        private void OnDestroy() {
            _powerUps.OnPowerUpApplied -= PowerUpApplied;
            _powerUps.OnAppliedPowerUpRemoved -= PowerUpRemoved;
        }

        private void PowerUpApplied(PowerUpComponent component) {
            _logger.Log("Power up applied: "+component);
            var infoGameObject = Instantiate(infoTemplate, transform);
            var info = infoGameObject.GetComponent<AppliedPowerUpInfo>();
            info.powerUp = component;
            _infos[component] = infoGameObject;
            infoGameObject.SetActive(true);
        }

        private void PowerUpRemoved(PowerUpComponent component) {
            _logger.Log("Power up removed: "+component);
            Destroy(_infos[component]);
            _infos.Remove(component);
        }
    }
}
