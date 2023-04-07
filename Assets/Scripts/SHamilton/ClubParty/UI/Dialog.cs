using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI {
    public class Dialog : MonoBehaviour {
        [SerializeField] private bool debug;
        [SerializeField] private float animTime = 0.5f;
        [SerializeField] private BackgroundPanel background;
        [SerializeField] private RectTransform dialogBox;
        public TMP_Text title;
        public TMP_Text content;

        private Logger _logger;
        private bool _okClicked;
	
        private void Start() {
            _logger = new(this, debug);
            
            dialogBox.localScale = Vector2.zero;
            LeanTween.scale(dialogBox, Vector2.one, animTime).setEaseOutCubic();
        }

        [UsedImplicitly]
        public void OkClicked() {
            if (_okClicked) return;
            _okClicked = true;

            background.FadeOut();
            LeanTween.scale(dialogBox, Vector2.zero, animTime).setEaseInCubic().setOnComplete(() => {
                Destroy(gameObject);
            });
        }
    }
}

