using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using SHamilton.ClubParty.Network;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.Ball {
    /// <summary>
    /// Sets the player's color
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(Renderer))]
    public class PlayerColor : MonoBehaviour {
    
        [SerializeField] private bool debug;

        private Logger _logger;
        private PhotonView _view;
        private Renderer _renderer;
	
        private void Start() {
            _logger = new(this, debug);
            _view = GetComponent<PhotonView>();
            _renderer = GetComponent<Renderer>();

            UpdateColor();
            NetworkManager.onPlayerPropertiesChanged += PlayerPropertiesChanged;
            
            if (_view.IsMine && NetworkManager.LocalPlayer.GetCharacterColor() == null) {
                NetworkManager.LocalPlayer.SetCharacterColor(Prefs.CharacterColor);
            }
        }

        private void OnDestroy() {
            NetworkManager.onPlayerPropertiesChanged -= PlayerPropertiesChanged;
        }

        private void UpdateColor() {
            var color = _view.Owner.GetCharacterColor();
            _logger.Log("Changing color to "+color);
            _renderer.material.color = color ?? Color.white;
        }

        private void PlayerPropertiesChanged(Player player, Hashtable changedProperties) {
            if (!changedProperties.ContainsKey(PropertyKeys.CharacterColor)) return;
            UpdateColor();
        }
    }
}
