using SHamilton.ClubParty.Network;
using TMPro;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI {
    public class Scoreboard : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private float timer = 5;
        [SerializeField] private GameObject scoreboard;
        [SerializeField] private GameObject scoreboardPlayerPrefab;
        [SerializeField] private Transform playersContainer;
        [SerializeField] private TextMeshProUGUI[] parScores;
        [SerializeField] private TextMeshProUGUI holeNameText;

        private Logger _logger;
        private bool _scoreboardShown;
	
        private void Awake() {
            _logger = new(this, debug);
            GameManager.OnLevelFinished += ShowScoreboard;
        }

        private void OnDestroy() {
            GameManager.OnLevelFinished -= ShowScoreboard;
        }

        private void Update() {
            if (!_scoreboardShown) return;
            
            timer -= Time.deltaTime;
            if (timer <= 0) {
                _logger.Log("Timer finished. Loading voting screen...");
                timer = 15;
                GameManager.StartGame();
            }
        }

        private void ShowScoreboard() {
            _logger.Log("Showing scoreboard...");
            _scoreboardShown = true;
            for (int i = 0; i < parScores.Length; i++) {
                var par = GameManager.Instance.holes[i].Par;
                parScores[i].text = par.ToString();
            }
            holeNameText.text = GameManager.CurrentCourse.courseName;

            foreach(var player in NetworkManager.Players) {
                var scoreboardPlayer = Instantiate(scoreboardPlayerPrefab, playersContainer);
                scoreboardPlayer.SetActive(true);
                scoreboardPlayer.GetComponent<ScoreboardPlayer>().SetPlayer(player);
            }
            
            scoreboard.SetActive(true);
        }
        
    }
}
