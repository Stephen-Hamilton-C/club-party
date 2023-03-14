using System;
using JetBrains.Annotations;
using Photon.Pun;
using SHamilton.ClubParty.Ball;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.PowerUp.HoleMagnet {
    [RequireComponent(typeof(PhotonView))]
    public class HoleMagnetPowerUp : MonoBehaviour {
    
        [SerializeField] private bool debug;

        private Logger _logger;
        private PhotonView _view;
        private GameObject _magnet;
	
        private void Start() {
            _logger = new(this, debug);
            _view = GetComponent<PhotonView>();
            
            if (_view.IsMine) {
                // Apply effect
                var magnetResource = Resources.Load<GameObject>("PowerUp/HoleMagnet");
                var hole = GameManager.Instance.CurrentHole.hole.transform;
                _magnet = Instantiate(magnetResource, hole.position, Quaternion.identity);

                LocalPlayerState.OnStrokeFinished += StrokeFinished;
            }
        }

        private void StrokeFinished() {
            _view.RPC("RemoveHoleMagnetRPC", RpcTarget.AllBuffered);
        }

        private void OnDestroy() {
            // Reset effect
            if(_magnet != null)
                Destroy(_magnet);

            LocalPlayerState.OnStrokeFinished -= StrokeFinished;
        }

        [PunRPC, UsedImplicitly]
        private void RemoveHoleMagnetRPC() {
            Destroy(this);
        }
    }
}

