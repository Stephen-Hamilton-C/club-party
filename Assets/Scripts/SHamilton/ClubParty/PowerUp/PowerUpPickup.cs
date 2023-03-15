using System.Linq;
using JetBrains.Annotations;
using Photon.Pun;
using SHamilton.ClubParty.Network;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.PowerUp {
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(PhotonView))]
    public class PowerUpPickup : MonoBehaviour {

        private PowerUpData[] _powerUps;
        
        [SerializeField] private bool debug;

        private PhotonView _view;
        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
            _view = GetComponent<PhotonView>();
            _powerUps = StoredPowerUps.PowerUpDatas.Values.ToArray();
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;

            _logger.Log("Player (" + other.name + ") touched this pickup.");

            var otherView = other.GetComponent<PhotonView>();
            if(otherView.Owner.IsLocal) {
                _logger.Log("Player who touched is the local player. Picking up...");
                var storedPowerUps = other.GetComponent<StoredPowerUps>();
                var selectedPowerUp = _powerUps[Random.Range(0, _powerUps.Length)];
                _logger.Log("Selected " + selectedPowerUp.Name);
                storedPowerUps.Add(selectedPowerUp);
                if (_view.Owner.IsLocal) {
                    NetworkManager.Destroy(gameObject);
                } else {
                    _view.RPC("DestroyRPC", _view.Owner);
                    Destroy(gameObject);
                }
            }
        }

        [PunRPC, UsedImplicitly]
        private void DestroyRPC() {
            NetworkManager.Destroy(gameObject);
        }
    }
}

