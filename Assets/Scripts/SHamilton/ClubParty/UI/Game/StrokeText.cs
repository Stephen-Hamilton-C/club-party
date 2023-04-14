using SHamilton.ClubParty.Ball;
using SHamilton.ClubParty.Network;
using TMPro;
using UnityEngine;

namespace SHamilton.ClubParty.UI.Game {
    [RequireComponent(typeof(TMP_Text))]
    public class StrokeText : MonoBehaviour  {
        private TMP_Text _text;
        private ScoreTracker _scoreTracker;

        private void Start() {
            _text = GetComponent<TMP_Text>();
            _scoreTracker = NetworkManager.LocalCharacter.GetComponent<ScoreTracker>();
        }

        private void Update() {
            _text.text = "Stroke: " + _scoreTracker.Strokes;
        }
    }
}
