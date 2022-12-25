using System.Linq;
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
            var playerScores = (int[])player.CustomProperties["Scores"];
            
            playerName.text = player.NickName;
            for (int i = 0; i < scores.Length; i++) {
                scores[i].text = (playerScores[i] + GameManager.Instance.holes[i].Par).ToString();
            }
            totalScore.text = playerScores.Sum().ToString();
        }
    
    }
}
