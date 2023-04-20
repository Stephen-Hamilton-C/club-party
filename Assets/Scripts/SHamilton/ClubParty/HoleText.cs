using Photon.Realtime;
using SHamilton.ClubParty.Ball;
using SHamilton.ClubParty.Network;
using TMPro;
using UnityEngine;

namespace SHamilton.ClubParty {
    public class HoleText : MonoBehaviour {
        [SerializeField] private float moveTo;
        [SerializeField] private float animTime;
        [SerializeField] private float visibleTime;
        private TMP_Text _text;
        private Hole _hole;
        private float _finishedTime;
        
        private void Start() {
            _text = GetComponentInChildren<TMP_Text>();
            _hole = GetComponentInParent<HoleTrigger>().hole;
            
            GameManager.OnPlayerFinished += PlayerFinishedHole;
        }

        private void OnDestroy() {
            GameManager.OnPlayerFinished -= PlayerFinishedHole;
        }

        private void PlayerFinishedHole(Player player) {
            if(!player.IsLocal) return;
            if(!_hole.isCurrent) return;
            
            var scores = NetworkManager.LocalCharacter.GetComponent<ScoreTracker>();
            if(scores.Strokes <= 1) {
                _text.text = "ACE!";
            } else if(scores.CurrentScore == 1) {
                _text.text = "Bogey";
            } else if(scores.CurrentScore == 2) {
                _text.text = "Double Bogey";
            } else if(scores.CurrentScore == 3) {
                _text.text = "Triple Bogey";
            } else if(scores.CurrentScore == 0) {
                _text.text = "Par";
            } else if(scores.CurrentScore == -1) {
                _text.text = "Birdie!";
            } else if(scores.CurrentScore == -2) {
                _text.text = "Eagle!";
            } else if(scores.CurrentScore == -3) {
                _text.text = "Double Eagle!";
            } else {
               var positiveSign = scores.CurrentScore > 0 ? "+" : "";
               _text.text = positiveSign + scores.CurrentScore;
            }
            
            LeanTween.moveLocalZ(gameObject, moveTo, animTime).setEaseOutQuad();
            _finishedTime = Time.time;
        }

        private void Update() {
            if(_finishedTime == 0) return;
            if(Time.time - _finishedTime >= visibleTime + animTime) {
                var color = _text.color;
                color.a -= Time.deltaTime / animTime;
                _text.color = color;
                if(color.a <= 0) {
                    Destroy(this);
                }
            }
        }
    }
}
