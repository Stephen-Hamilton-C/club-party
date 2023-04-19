using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon.Pun;
using SHamilton.ClubParty.PowerUp.HoleMagnet;
using SHamilton.ClubParty.PowerUp.Hyperball;

namespace SHamilton.ClubParty.PowerUp {
    [RequireComponent(typeof(PhotonView))]
    public class StoredPowerUps : MonoBehaviour {
        public event PowerUpComponent.PowerUpEvent OnPowerUpApplied;
        public event PowerUpComponent.PowerUpEvent OnAppliedPowerUpRemoved;
        
        public static readonly Dictionary<string, PowerUpData> PowerUpDatas = new() {
            // NOTE: It's very important that the key exactly matches the Name property
            {"Hyperball", new HyperballData()},
            {"Hole Magnet", new HoleMagnetData()},
        };

        [SerializeField] private int maxPowerUps = 3;

        public int MaxPowerUps => maxPowerUps;
        public IReadOnlyList<PowerUpData> PowerUps => _powerUps;
        private readonly List<PowerUpData> _powerUps = new();
        /// <summary>
        /// NOTE: This works only on the LocalCharacter!
        /// </summary>
        public IReadOnlyList<PowerUpComponent> AppliedPowerUps => _appliedPowerUps;
        private readonly List<PowerUpComponent> _appliedPowerUps = new();

        private PhotonView _view;

        private void Start() {
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
            if (TryGetComponent(powerUp.ComponentType, out var existingComponent)) {
                var powerUpComponent = existingComponent as PowerUpComponent;
                powerUpComponent!.Amount++;
            } else {
                var addedComponent = gameObject.AddComponent(powerUp.ComponentType) as PowerUpComponent;
                addedComponent!.Data = powerUp;
                _appliedPowerUps.Add(addedComponent);
                addedComponent!.OnPowerUpDestroyed += PowerUpDestroyed;
                
                OnPowerUpApplied?.Invoke(addedComponent);
            }
        }

        private void PowerUpDestroyed(PowerUpComponent component) {
            _appliedPowerUps.Remove(component);
            component.OnPowerUpDestroyed -= PowerUpDestroyed;
            
            OnAppliedPowerUpRemoved?.Invoke(component);
        }
    }
}