using UnityEngine;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI.Flair {
    [RequireComponent(typeof(Graphic))]
    [ExecuteInEditMode]
    public class SelectableContrast : MonoBehaviour {
        private Graphic _graphic;
        private SelectableColor _parentColor;
        
        private void Start() {
            _graphic = GetComponent<Graphic>();
            _parentColor = GetComponentInParent<SelectableColor>();
            _parentColor.OnSpritesChanged += SpritesChanged;
        }

        private void OnDestroy() {
            _parentColor.OnSpritesChanged -= SpritesChanged;
        }

        private void SpritesChanged(SelectableSprites sprites) {
            _graphic.color = sprites.textColor;
        }
    }
}
