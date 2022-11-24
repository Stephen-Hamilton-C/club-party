using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Button))]
    public abstract class ButtonBase : MonoBehaviour {

        [SerializeField] protected bool debug;

        private Logger _logger;
        protected Button Button;
        protected TextMeshProUGUI ButtonText;

        protected virtual void Start() {
            _logger = new(this, debug);
            Button = GetComponent<Button>();
            Button.onClick.AddListener(OnClick);
            ButtonText = GetComponentInChildren<TextMeshProUGUI>();
        }

        protected abstract void OnClick();
    }
}
