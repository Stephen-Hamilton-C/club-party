using JetBrains.Annotations;
using Network;
using Photon.Pun;
using UnityEngine;

namespace Ball.PowerUps {
    [RequireComponent(typeof(PhotonView))]
    public abstract class PowerUp : MonoBehaviour {

        [SerializeField] protected bool debug;
        
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
            _logger = new(this, debug);
            LocalPlayerState.OnStroke += Stroked;
            NetworkManager.onLocalCharacterInitialized += LocalCharacterInitialized;

            View = GetComponent<PhotonView>();
            View.OwnershipTransfer = OwnershipOption.Takeover;

            _collider = GetComponent<Collider>();
            _renderer = GetComponent<Renderer>();
        }

        private void LocalCharacterInitialized() {
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
            LocalPlayerState.OnStroke -= Stroked;
            NetworkManager.onLocalCharacterInitialized -= LocalCharacterInitialized;
        }
        
        [PunRPC]
        [UsedImplicitly]
        protected virtual void HideRPC() {
            _collider.enabled = false;
            _renderer.enabled = false;
        }
        
    }
}
