using UnityEngine;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI {
    [RequireComponent(typeof(Image))]
    [ExecuteInEditMode]
    public class ImageContrast : Contrast {
        private Image _image;

        protected override void Start() {
            base.Start();
            _image = GetComponent<Image>();
        }

        private void Update() {
            _image.color = ForegroundColor;
        }
    }
}

