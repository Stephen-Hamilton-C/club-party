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
        [SerializeField] private Color positiveScoreColor;
        [SerializeField] private Color negativeScoreColor;

        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
        }

        public void SetPlayer(Player player) {
            // var playerScores = new PlayerProperties(player).Scores;
            var playerScores = (int[]) player.CustomProperties[PropertyKeys.Scores];
            
            playerName.text = player.NickName;
            for (int i = 0; i < scores.Length; i++) {
                var score = playerScores[i];
                var strokes = score + GameManager.Instance.holes[i].Par;
                var sign = "";
                var color = scores[i].color;

                if(score > 0) {
                    color = positiveScoreColor;
                } else if(score < 0) {
                    color = negativeScoreColor;
                }
                scores[i].text = sign + strokes;
                scores[i].color = color;
            }
            totalScore.text = playerScores.Sum().ToString();
        }
    
    }
}
