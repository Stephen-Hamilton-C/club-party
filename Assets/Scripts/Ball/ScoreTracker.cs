using System.Collections.Generic;
using UnityEngine;

namespace Ball {
    public class ScoreTracker : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private int lateJoinScore;

	// TODO: Store this with PhotonPlayer.CustomSettings instead
        public IReadOnlyList<int> Scores => _scores;
        private int[] _scores;

        private Logger _logger;
        private Hole[] Holes => GameManager.Instance.holes;
	
        private void Start() {
            _logger = new(this, debug);
            
            _scores = new int[Holes.Length];
            for (int i = 0; i < _scores.Length; i++) {
                // Set score to lateJoinScore for holes the player never played if they join late
                _scores[i] = i >= GameManager.HoleIndex ? -Holes[i].Par : lateJoinScore;
            }
            
            _logger.Log("Scores initialized to "+string.Join(", ", _scores));

            LocalPlayerState.OnStroke += PlayerStroked;
        }

        private void OnDestroy() {
            LocalPlayerState.OnStroke -= PlayerStroked;
        }

        private void PlayerStroked() {
            _scores[GameManager.HoleIndex]++;
            _logger.Log("Scores set to "+string.Join(", ", _scores));
        }

    }
}
