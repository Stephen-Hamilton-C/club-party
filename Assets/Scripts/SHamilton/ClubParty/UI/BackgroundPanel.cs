using UnityEngine;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI {
    [RequireComponent(typeof(CanvasGroup))]
    [AddComponentMenu("UI/Background Panel")]
    public class BackgroundPanel : Image {
        public bool animate;
        public float time = 0.5f;
        public float delay = 0f;

        private CanvasGroup _canvas;

        #if UNITY_EDITOR
        protected override void Reset() {
            base.Reset();
            color = Color.black * 0.1f;
        }
        #endif

        protected override void Awake() {
            base.Awake();
            _canvas = GetComponent<CanvasGroup>();
        }

        protected override void OnEnable() {
            base.OnEnable();
            if (Application.isPlaying) {
                _canvas.alpha = 0;
                LeanTween.value(gameObject, UpdateBackground, 0, 1, time).setDelay(delay);
            }
        }

        public void FadeOut() {
            if (Application.isPlaying) {
                _canvas.alpha = 1;
                LeanTween.value(gameObject, UpdateBackground, 1, 0, time);
            }
        }

        private void UpdateBackground(float value) {
            _canvas.alpha = value;
        }
    }
}

