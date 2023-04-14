using ExitGames.Client.Photon;
using Photon.Pun;
using SHamilton.ClubParty.Network;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.Ball {
    [RequireComponent(typeof(PhotonView))]
    public class ScoreTracker : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private int lateJoinScore;

        public int Strokes => _scores[GameManager.HoleIndex] + GameManager.Instance.CurrentHole.Par;

        private int[] _scores;

        private Logger _logger;
        private PhotonView _view;
        private Hole[] Holes => GameManager.Instance.holes;
	
        private void Start() {
            _logger = new(this, debug);
            _view = GetComponent<PhotonView>();
            if(!_view.IsMine)
                Destroy(this);
            
            _scores = new int[Holes.Length];
            for (int i = 0; i < _scores.Length; i++) {
                // Set score to lateJoinScore for holes the player never played if they join late
                _scores[i] = i >= GameManager.HoleIndex ? -Holes[i].Par : lateJoinScore;
            }

            UpdateScores();
            _logger.Log("Scores initialized to "+string.Join(", ", _scores));

            LocalPlayerState.OnStroke += PlayerStroked;
        }

        private void OnDestroy() {
            LocalPlayerState.OnStroke -= PlayerStroked;
        }

        private void UpdateScores() {
            // NetworkManager.LocalPlayerProperties.Scores = _scores;
            NetworkManager.LocalPlayer.CustomProperties[PropertyKeys.Scores] = _scores;
            var propChanges = new Hashtable() { { PropertyKeys.Scores, _scores } };
            NetworkManager.LocalPlayer.SetCustomProperties(propChanges);
        }

        private void PlayerStroked() {
            _scores[GameManager.HoleIndex]++;
            UpdateScores();
            _logger.Log("Scores set to "+string.Join(", ", _scores));
        }

    }
}
