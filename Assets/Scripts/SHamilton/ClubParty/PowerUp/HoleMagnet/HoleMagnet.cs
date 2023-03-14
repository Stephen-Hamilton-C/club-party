using SHamilton.ClubParty.Network;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.PowerUp.HoleMagnet {
    public class HoleMagnet : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private float range = 2f;
        [SerializeField] private float cutoff = 0.25f;
        [SerializeField] private float strength = 1f;

        private Logger _logger;
        private Transform _character;
        private Rigidbody _charRb;
	
        private void Start() {
            _logger = new(this, debug);
            _character = NetworkManager.LocalCharacter.transform;
            _charRb = _character.GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            var charPos = _character.position;
            var magnetPos = transform.position;
            
            var distance = (charPos - magnetPos).magnitude;
            if (distance <= range && distance >= cutoff) {
                // We're assuming this magnet has a mass of 1, so we only need the character's mass
                var magnitude = strength * _charRb.mass / (distance * distance);
                var direction = (magnetPos - charPos).normalized;
                var force = magnitude * direction;
                // Don't allow the ball to float back up out of the hole
                if (force.y > 0)
                    force.y = 0;
                _charRb.AddForce(force, ForceMode.Force);
            }
        }
    }
}

