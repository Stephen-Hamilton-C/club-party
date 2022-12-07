using Photon.Pun;
using UnityEngine;

// TODO: Maybe this should be split into two classes
// One that gets applied to players
// And a second that is used for environmental powerups (like tornado)
namespace Ball.PowerUps {
    [RequireComponent(typeof(PhotonView))]
    public abstract class PowerUp : MonoBehaviour {
    
        [SerializeField] protected bool debug;
        
        public abstract string PowerUpName { get; }
        public abstract string PowerUpDescription { get; }

        private PowerUpManager _powerUpMan;
	
        private void Start() {
            _powerUpMan = GetComponent<PowerUpManager>();
            ApplyEffect();
        }

        protected abstract void ApplyEffect();
        protected abstract void RemoveEffect();

        private void OnDestroy() {
            RemoveEffect();
            _powerUpMan.PowerUpFinished(this.GetType());
        }
    }
}

