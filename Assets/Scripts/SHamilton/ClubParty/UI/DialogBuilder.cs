using UnityEngine;

namespace SHamilton.ClubParty.UI {
    public class DialogBuilder {
        private static GameObject _loadedDialog;
        private string _title = "Information";
        private string _content = "No message provided";

        public DialogBuilder() {
            if(_loadedDialog == null)
                _loadedDialog = Resources.Load<GameObject>("UI/Dialog");
        }

        public DialogBuilder SetTitle(string title) {
            _title = title;
            return this;
        }

        public DialogBuilder SetContent(string content) {
            _content = content;
            return this;
        }

        public Dialog Build() {
            var dialog = Object.Instantiate(_loadedDialog).GetComponent<Dialog>();
            dialog.title.text = _title;
            dialog.content.text = _content;
            return dialog;
        }
    }
}

