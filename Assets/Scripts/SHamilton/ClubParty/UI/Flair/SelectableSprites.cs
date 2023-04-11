using UnityEngine;

namespace SHamilton.ClubParty.UI.Flair {
    [CreateAssetMenu(fileName = "Sprites", menuName = "ScriptableObjects/SelectableSprites")]
    public class SelectableSprites : ScriptableObject {
        public Sprite idle;
        public Sprite highlighted;
        public Sprite pressed;
        public Sprite disabled;
        public Color textColor;
    }
}
