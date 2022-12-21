using System.Linq;
using UnityEngine;

// TODO: Handle when player joins and we're still at spawn
namespace Ball {
    public class PlayerCollision : MonoBehaviour {
    
        [SerializeField] private bool debug;

        private bool _levelFinished;
        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
            GameManager.OnHoleFinished += HoleFinished;
        }

        private void OnDestroy() {
            GameManager.OnHoleFinished -= HoleFinished;
        }

        private void HoleFinished() {
            _logger.Log("Hole Finished");
            _levelFinished = true;
        }

        private void Update() {
            if (gameObject.layer != LayerMask.NameToLayer("PlayerNoCollide")) return;
            if (!_levelFinished) return;

            var results = new Collider[10];
            Physics.OverlapSphereNonAlloc(transform.position, transform.localScale.x, results);
            var otherPlayers = results.Where(other => other?.CompareTag("Player") ?? false).ToArray();
            _logger.Log(gameObject.name+" is currently inside "+otherPlayers.Length+" player(s)");
            if (otherPlayers.Length == 0) {
                _logger.Log(gameObject.name+" has left all players. Setting layer to default.");
                _levelFinished = false;
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }

    }
}
