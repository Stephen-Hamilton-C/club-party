using JetBrains.Annotations;
using Photon.Pun;
using SHamilton.ClubParty.Ball;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.PowerUp.Hyperball {
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PhotonView))]
    public class HyperballPowerUp : MonoBehaviour {

        private const float SpeedFactor = 10f;
        
        [SerializeField] private bool debug;

        private Logger _logger;
        private PhotonView _view;
        private PlayerController _controller;
        private float _origSpeed;
	
        private void Start() {
            _logger = new(this, debug);
            _view = GetComponent<PhotonView>();
            _controller = GetComponent<PlayerController>();

            if (_view.IsMine) {
                // Apply Hyperball
                _origSpeed = _controller.speed;
                _controller.speed *= SpeedFactor;
                LocalPlayerState.OnStroke += Stroked;
            }
        }

        private void OnDestroy() {
            // Reset state
            _controller.speed = _origSpeed;
        }

        private void Stroked() {
            LocalPlayerState.OnStroke -= Stroked;
            _view.RPC("RemoveHyperballRPC", RpcTarget.AllBuffered);
        }

        [PunRPC, UsedImplicitly]
        private void RemoveHyperballRPC() {
            Destroy(this);
        }
    }
}

