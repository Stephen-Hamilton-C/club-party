using Photon.Pun;
using TMPro;
using UnityEngine;

namespace UI {
    public class Scoreboard : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private float timer = 5;
        [SerializeField] private GameObject scoreboardPlayerPrefab;
        [SerializeField] private Transform playersContainer;
        [SerializeField] private TextMeshProUGUI[] parScores;

        private Logger _logger;
	
        private void Awake() {
            _logger = new(this, debug);
            GameManager.OnLevelFinished += ShowScoreboard;
            gameObject.SetActive(false);
        }

        private void OnDestroy() {
            GameManager.OnLevelFinished -= ShowScoreboard;
        }

        private void Update() {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                _logger.Log("Timer finished. Loading voting screen...");
                timer = 15;
                GameManager.StartGame();
            }
        }

        private void ShowScoreboard() {
            _logger.Log("Showing scoreboard...");
            for (int i = 0; i < parScores.Length; i++) {
                parScores[i].text = GameManager.Instance.holes[i].Par.ToString();
            }
            
            foreach(var player in PhotonNetwork.PlayerList) {
                var scoreboardPlayer = Instantiate(scoreboardPlayerPrefab, playersContainer);
                scoreboardPlayer.GetComponent<ScoreboardPlayer>().SetPlayer(player);
            }
            
            gameObject.SetActive(true);
        }
        
    }
}
