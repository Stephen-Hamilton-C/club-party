using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

namespace Ball.PowerUps {
    [RequireComponent(typeof(PhotonView))]
    public abstract class PowerUp : MonoBehaviour {

        public abstract string PowerUpName { get; protected set; }
        public abstract string PowerUpDescription { get; protected set; }

        protected GameObject LocalCharacter;
        protected PowerUpManager Manager;
        protected PhotonView View;
        protected bool Triggered;
        
        private Logger _logger;
        private Collider _collider;
        private Renderer _renderer;

        protected virtual void Awake() {
            _logger = new(this, Application.isEditor);
            PlayerState.OnStroke += Stroked;

            View = GetComponent<PhotonView>();
            View.OwnershipTransfer = OwnershipOption.Takeover;

            _collider = GetComponent<Collider>();
            _renderer = GetComponent<Renderer>();
        }

        protected virtual void Start() {
            LocalCharacter = PhotonNetwork.LocalPlayer.CustomProperties["Character"] as GameObject;
            Manager = LocalCharacter.GetComponent<PowerUpManager>();
        }

        private void OnTriggerEnter(Collider other) {
            if (!Triggered && other.CompareTag("Player")) {
                Triggered = true;
                if (other.gameObject == LocalCharacter) {
                    View.RPC("HideRPC", RpcTarget.AllBuffered);
                    View.TransferOwnership(PhotonNetwork.LocalPlayer);
                    OnLocalPlayerEntered();
                }
            }
        }

        protected abstract void OnLocalPlayerEntered();

        protected virtual void Stroked() {
            _logger.Log("Stroked was not overridden for "+PowerUpName);
        }

        private void OnDestroy() {
            PlayerState.OnStroke -= Stroked;
        }
        
        [PunRPC]
        [UsedImplicitly]
        protected virtual void HideRPC() {
            _collider.enabled = false;
            _renderer.enabled = false;
        }
        
    }
}
