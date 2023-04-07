using UnityEngine;
using TMPro;

namespace SHamilton.ClubParty.UI {
    [RequireComponent(typeof(TMP_Text))]
    [ExecuteInEditMode]
    public class TextContrast : Contrast {
        private TMP_Text _text;
        
        protected override void Start() {
            base.Start();
            _text = GetComponent<TMP_Text>();
        }

        public void Update() {
            var color = ForegroundColor;
            color.a = _text.color.a;
            _text.color = color;
        }
    }
}

