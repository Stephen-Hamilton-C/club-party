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
        [SerializeField] private Color aceColor;
        [SerializeField] private Color goodColor;
        [SerializeField] private Color parColor;
        [SerializeField] private Color badColor;

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
                _text.color = aceColor;
            } else if(scores.CurrentScore == 1) {
                _text.text = "Bogey";
                _text.color = badColor;
            } else if(scores.CurrentScore == 2) {
                _text.text = "Double Bogey";
                _text.color = badColor;
            } else if(scores.CurrentScore == 3) {
                _text.text = "Triple Bogey";
                _text.color = badColor;
            } else if(scores.CurrentScore == 0) {
                _text.text = "Par";
                _text.color = parColor;
            } else if(scores.CurrentScore == -1) {
                _text.text = "Birdie!";
                _text.color = goodColor;
            } else if(scores.CurrentScore == -2) {
                _text.text = "Eagle!";
                _text.color = goodColor;
            } else if(scores.CurrentScore == -3) {
                _text.text = "Double Eagle!";
                _text.color = goodColor;
            } else {
                var isScorePositive = scores.CurrentScore > 0;
                var positiveSign = isScorePositive ? "+" : "";
                _text.text = positiveSign + scores.CurrentScore;
                _text.color = isScorePositive ? badColor : goodColor;
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
