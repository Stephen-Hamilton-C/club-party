using UnityEngine;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI.Flair {
    [RequireComponent(typeof(Image))]
    [ExecuteInEditMode]
    public class SelectableColorChild : MonoBehaviour {
        private Image _image;
        private SelectableColor _parent;

        private void Start() {
            _image = GetComponent<Image>();
            _parent = GetComponentInParent<SelectableColor>();
            _parent.OnSpritesChanged += SpritesChanged;
        }

        private void OnDestroy() {
            _parent.OnSpritesChanged -= SpritesChanged;
        }

        private void SpritesChanged(SelectableSprites sprites) {
            _image.sprite = sprites.idle;
        }
    }
}