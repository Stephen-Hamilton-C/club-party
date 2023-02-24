using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon.Pun;
using SHamilton.ClubParty.PowerUp.Hyperball;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.PowerUp {
    [RequireComponent(typeof(PhotonView))]
    public class StoredPowerUps : MonoBehaviour {

        public static readonly Dictionary<string, PowerUpData> PowerUpDatas = new() {
            // NOTE: It's very important that the key exactly matches the Name property
            {"Hyperball", new HyperballData()},
        };

        [SerializeField] private bool debug;
        [SerializeField] private int maxPowerUps = 3;

        public int MaxPowerUps => maxPowerUps;
        public IReadOnlyList<PowerUpData> PowerUps => _powerUps;
        private readonly List<PowerUpData> _powerUps = new();

        private PhotonView _view;
        private Logger _logger;

        private void Start() {
            _logger = new(this, debug);
            _view = GetComponent<PhotonView>();
        }

        public void Add(PowerUpData powerUp) {
            _view.RPC("AddPowerUpRPC", _view.Owner, powerUp.Name);
        }

        [PunRPC, UsedImplicitly]
        private void AddPowerUpRPC(string powerUpName) {
            if (_powerUps.Count >= maxPowerUps) return;
            
            var powerUp = PowerUpDatas[powerUpName];
            _powerUps.Add(powerUp);
        }

        public bool Remove(PowerUpData powerUp) {
            return _powerUps.Remove(powerUp);
        }

        [PunRPC, UsedImplicitly]
        private void AddPowerUpComponentRPC(string powerUpName) {
            var powerUp = PowerUpDatas[powerUpName];
            // Don't apply if already applied
            if (TryGetComponent(powerUp.ComponentType, out _)) return;
            
            gameObject.AddComponent(powerUp.ComponentType);
        }

    }
}