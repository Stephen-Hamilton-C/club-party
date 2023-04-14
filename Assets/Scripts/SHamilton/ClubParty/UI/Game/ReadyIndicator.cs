using SHamilton.ClubParty.Ball;
using UnityEngine;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI.Game {
    /// <summary>
    /// Simple prototype indicator to alert the player when they can hit the ball
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ReadyIndicator : MonoBehaviour {

        [SerializeField] private Sprite notReady;
        [SerializeField] private Sprite ready;
        
        private Image _image;

        private void Start() {
            _image = GetComponent<Image>();
            
            UpdateColor(LocalPlayerState.CanStroke);
            LocalPlayerState.OnCanStrokeChanged += UpdateColor;
        }

        private void UpdateColor(bool canStroke) {
            _image.sprite = canStroke ? ready : notReady;
        }

        private void OnDestroy() {
            LocalPlayerState.OnCanStrokeChanged -= UpdateColor;
        }
    }
}
