using System.Linq;
using Photon.Realtime;
using SHamilton.ClubParty.Network;
using TMPro;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI {
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
            // var playerScores = new PlayerProperties(player).Scores;
            var playerScores = (int[]) player.CustomProperties[PropertyKeys.Scores];
            
            playerName.text = player.NickName;
            for (int i = 0; i < scores.Length; i++) {
                scores[i].text = (playerScores[i] + GameManager.Instance.holes[i].Par).ToString();
            }
            totalScore.text = playerScores.Sum().ToString();
        }
    
    }
}
