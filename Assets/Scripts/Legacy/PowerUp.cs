using System;
using Ball;
using JetBrains.Annotations;
using Network;
using Photon.Pun;
using UnityEngine;

namespace Legacy {
    /// <summary>
    /// The base class for all PowerUps. Handles retrieving common properties and procedures
    /// before letting the power up have specific control
    /// </summary>
    [Obsolete("To be replaced with new PowerUp design")]
    [RequireComponent(typeof(PhotonView))]
    public abstract class PowerUp : MonoBehaviour {

        [SerializeField] protected bool debug;
        
        public abstract string PowerUpName { get; protected set; }
        public abstract string PowerUpDescription { get; protected set; }

        /// <summary>
        /// The reference to the local player character
        /// </summary>
        protected GameObject LocalCharacter;
        /// <summary>
        /// The local character's PowerUpManager
        /// </summary>
        protected PowerUpManager Manager;
        /// <summary>
        /// The PhotonView attached to this PowerUp
        /// </summary>
        protected PhotonView View;
        /// <summary>
        /// Whether this PowerUp was touched by any player
        /// </summary>
        protected bool Triggered;
        
        private Logger _logger;
        /// <summary>
        /// The trigger collider on this PowerUp
        /// </summary>
        private Collider _collider;
        /// <summary>
        /// The renderer on this PowerUp
        /// </summary>
        private Renderer _renderer;

        protected virtual void Awake() {
            _logger = new(this, debug);
            
            // Subscribe to events
            LocalPlayerState.OnStroke += Stroked;

            // Setup PhotonView
            // Needs Takeover ownership so that the PowerUp can be transferred to the affected player
            View = GetComponent<PhotonView>();
            View.OwnershipTransfer = OwnershipOption.Takeover;

            // Get Components
            _collider = GetComponent<Collider>();
            _renderer = GetComponent<Renderer>();
        }

        protected virtual void Start() {
            LocalCharacter = PhotonNetwork.LocalPlayer.GetCharacter();
            Manager = LocalCharacter.GetComponent<PowerUpManager>();
        }

        private void OnTriggerEnter(Collider other) {
            if (!Triggered && other.CompareTag("Player")) {
                // Any player touched this
                Triggered = true;
                if (other.gameObject == LocalCharacter) {
                    // The player that touched was the LocalPlayer
                    View.RPC("HideRPC", RpcTarget.AllBuffered);
                    View.TransferOwnership(PhotonNetwork.LocalPlayer);
                    OnLocalPlayerTouched();
                }
            }
        }

        protected abstract void OnLocalPlayerTouched();

        /// <summary>
        /// Helper for removing an effect on stroke, for PowerUps that take effect on next stroke
        /// </summary>
        protected virtual void Stroked() {
            _logger.Log("Stroked was not overridden for "+PowerUpName);
        }

        protected virtual void OnDestroy() {
            // Drop events
            LocalPlayerState.OnStroke -= Stroked;
        }
        
        [PunRPC]
        [UsedImplicitly]
        protected virtual void HideRPC() {
            _collider.enabled = false;
            _renderer.enabled = false;
        }
        
    }
}
