using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using SHamilton.ClubParty.Ball;
using UnityEngine;

namespace SHamilton.ClubParty.PowerUp.HoleMagnet {
    public class HoleMagnetPowerUp : PowerUpComponent {
    
        private GameObject _magnet;
        private GameObject _magnetResource;
        private bool _ignoreNextStroke;
	
        protected override void Start() {
            base.Start();
            
            if (View.IsMine) {
                // Apply effect
                _magnetResource = Resources.Load<GameObject>("PowerUp/HoleMagnet");
                CreateMagnet();

                LocalPlayerState.OnStrokeFinished += StrokeFinished;
                GameManager.OnPlayerFinished += PlayerFinished;
                GameManager.OnHoleFinished += CreateMagnet;
            }
        }

        private void CreateMagnet() {
            if(_magnet)
                Destroy(_magnet);

            var hole = GameManager.Instance.CurrentHole.hole.transform;
            _magnet = Instantiate(_magnetResource, hole.position, Quaternion.identity);
        }

        private void PowerUpFinished() {
            CreateMagnet();
            Amount--;
        }

        private void StrokeFinished() {
            if(_ignoreNextStroke) {
                _ignoreNextStroke = false;
                return;
            }

            PowerUpFinished();
        }
        
        private void PlayerFinished(Player player) {
            if (!player.IsLocal) return;
            
            // Account for edge case
            if(LocalPlayerState.Stroked)
                _ignoreNextStroke = true;
            
            PowerUpFinished();
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            
            // Reset effect
            if(_magnet)
                Destroy(_magnet);

            LocalPlayerState.OnStrokeFinished -= StrokeFinished;
            GameManager.OnPlayerFinished -= PlayerFinished;
            GameManager.OnHoleFinished -= CreateMagnet;
        }

        [PunRPC, UsedImplicitly]
        private void RemoveHoleMagnetRPC() {
            Destroy(this);
        }
    }
}

