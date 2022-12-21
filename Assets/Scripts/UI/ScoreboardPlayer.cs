using System.Linq;
using Ball;
using Network;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace UI {
    public class ScoreboardPlayer : MonoBehaviour {
    
        [SerializeField] private bool debug;

        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private TextMeshProUGUI[] scores;
        [SerializeField] private TextMeshProUGUI totalScore;

        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
        }

        public void SetPlayer(Player player) {
            var character = player.GetCharacter();
            var scoreTracker = character.GetComponent<ScoreTracker>();
            
            playerName.text = player.NickName;
            for (int i = 0; i < scores.Length; i++) {
                scores[i].text = (scoreTracker.Scores[i] + GameManager.Instance.holes[i].Par).ToString();
            }
            totalScore.text = scoreTracker.Scores.Sum().ToString();
        }
    
    }
}
