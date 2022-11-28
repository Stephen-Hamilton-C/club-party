using Ball;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game {
    /// <summary>
    /// Simple prototype indicator to alert the player when they can hit the ball
    /// </summary>
    [RequireComponent(typeof(RawImage))]
    public class ReadyIndicator : MonoBehaviour {

        private RawImage _image;

        private void Start() {
            _image = GetComponent<RawImage>();
            
            UpdateColor(LocalPlayerState.CanStroke);
            LocalPlayerState.OnCanStrokeChanged += UpdateColor;
        }

        private void UpdateColor(bool canStroke) {
            _image.color = canStroke ? Color.green : Color.red;
        }

        private void OnDestroy() {
            LocalPlayerState.OnCanStrokeChanged -= UpdateColor;
        }
    }
}
