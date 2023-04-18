using UnityEngine;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI {
    [RequireComponent(typeof(Selectable))]
    public class FocusOnEnable : MonoBehaviour {
        private Selectable _selectable;
        
        private void OnEnable() {
            if (!_selectable)
                _selectable = GetComponent<Selectable>();
            
            _selectable.Select();
        }
    }
}
