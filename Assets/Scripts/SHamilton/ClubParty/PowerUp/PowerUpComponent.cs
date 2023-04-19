using Photon.Pun;
using UnityEngine;

namespace SHamilton.ClubParty.PowerUp {
    [RequireComponent(typeof(PhotonView))]
    public abstract class PowerUpComponent : MonoBehaviour {
        public delegate void PowerUpEvent(PowerUpComponent component);
        public event PowerUpEvent OnPowerUpDestroyed;
        
        public int Amount {
            get => _amount;
            set {
                _amount = value;
                if (_amount <= 0) {
                    Destroy(this);
                }
            }
        }
        private int _amount = 1;

        public PowerUpData Data {
            get => _data;
            set {
                if (_data == null) {
                    _data = value;
                }
            }
        }
        private PowerUpData _data;

        protected PhotonView View;

        protected virtual void Start() {
            View = GetComponent<PhotonView>();
        }

        protected virtual void OnDestroy() {
            OnPowerUpDestroyed?.Invoke(this);
        }
    }
}
