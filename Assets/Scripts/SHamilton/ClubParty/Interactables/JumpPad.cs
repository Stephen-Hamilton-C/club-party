using System.Collections;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.Interactables {
    [RequireComponent(typeof(Animator))]
    public class JumpPad : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private float padForce = 5f;

        private Logger _logger;
        private Animator _animator;
        private static readonly int Boing = Animator.StringToHash("Boing");

        private void Start() {
            _logger = new(this, debug);
            _animator = GetComponent<Animator>();
        }

        private IEnumerator LaunchBall(Rigidbody rb) {
            // This is unfortunately super hardcoded, but here goes
            // The animation for the launcher takes 5 frames to go from down to up at 60 FPS
            // So, over the 5 (60 FPS) frames, add 1 / (5 - 1) to the y position
            // Because the framerate may not be exactly 60 FPS,
            // use WaitForSeconds to wait 1 / 60 seconds
            for (int i = 0; i < 5; i++) {
                var pos = rb.position;
                pos.y += 1 / 4f;
                rb.position = pos;
                yield return new WaitForSeconds(1f / 60);
            }

            var force = new Vector3(0, padForce, 0);
            
            // Prevent ball from getting stuck on pad by applying directional force as well
            var velocity = rb.velocity;
            velocity.y = 0;
            if (velocity.magnitude < 1) {
                _logger.Log("Player too slow, applying extra force: "+velocity.normalized);
                force += velocity.normalized;
            }

            rb.AddForce(force, ForceMode.Impulse);
            _logger.Log("Applied final jump force to "+rb.gameObject.name);
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;

            _logger.Log("Player "+other.gameObject.name+" hit jump pad");
            _animator.SetTrigger(Boing);
            StartCoroutine(nameof(LaunchBall), other.attachedRigidbody);
        }

    }
}
