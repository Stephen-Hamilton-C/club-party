using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace SHamilton.ClubParty.UI {
    public class Dialog : MonoBehaviour {
        public TMP_Text title;
        public TMP_Text content;

        public delegate void ClickedEvent();
        public event ClickedEvent OnOkClicked;

        [UsedImplicitly]
        public void OkClicked() {
            OnOkClicked?.Invoke();
            Destroy(gameObject);
        }
    }
}

