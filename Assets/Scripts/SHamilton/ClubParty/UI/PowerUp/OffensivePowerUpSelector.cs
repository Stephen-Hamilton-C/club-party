using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon.Realtime;
using SHamilton.ClubParty.Network;
using SHamilton.ClubParty.PowerUp;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.PowerUp {
    public class OffensivePowerUpSelector : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject playerTargetUITemplate;

        public static OffensivePowerUpSelector Instance => _instance;
        private static OffensivePowerUpSelector _instance;
        [CanBeNull] public static OffensivePowerUpData CurrentPowerUp => _instance._currentPowerUp;
        [CanBeNull] private OffensivePowerUpData _currentPowerUp = null;

        private Logger _logger;
        private readonly Dictionary<Player, GameObject> _playerTargetUIs = new();

        private void Awake() {
            _logger = new(this, debug);
            if (_instance != null) {
                _logger.Err("Another instance of OffensivePowerUpSelector exists! This duplicate instance will be destroyed.");
                Destroy(this);
            } else {
                _instance = this;
            }
            
            foreach (var player in NetworkManager.OtherPlayers) {
                CreatePlayerTargetUI(player);
            }

            NetworkManager.onPlayerJoined += CreatePlayerTargetUI;
            NetworkManager.onPlayerLeft += DestroyPlayerTargetUI;
        }

        private void OnDestroy() {
            NetworkManager.onPlayerJoined -= CreatePlayerTargetUI;
            NetworkManager.onPlayerLeft -= DestroyPlayerTargetUI;
        }

        public void OffensivePowerUpSelected(OffensivePowerUpData powerUp) {
            _logger.Log("PowerUp "+powerUp.Name+" selected!");
            _currentPowerUp = powerUp;
            panel.SetActive(true);
        }

        public void CloseSelector() {
            panel.SetActive(false);
            _currentPowerUp = null;
        }

        public void PlayerSelected(Player player) {
            _logger.Log("Player "+player+" selected!");
            _currentPowerUp!.ApplyToPlayer(player);
            CloseSelector();
        }

        private void CreatePlayerTargetUI(Player player) {
            var playerTargetUI = Instantiate(playerTargetUITemplate, playerTargetUITemplate.transform.parent);
            var playerTarget = playerTargetUI.GetComponent<PlayerTarget>();
            playerTarget.Player = player;
            playerTargetUI.SetActive(true);
            
            _playerTargetUIs[player] = playerTargetUI;
            _logger.Log("Created TargetPlayerUI for "+player);
        }

        private void DestroyPlayerTargetUI(Player player) {
            Destroy(_playerTargetUIs[player]);
            _playerTargetUIs.Remove(player);
            _logger.Log("Destroyed TargetPlayerUI for "+player);
        }
    }
}

