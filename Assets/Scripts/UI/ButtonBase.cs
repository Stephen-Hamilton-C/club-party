using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    /// <summary>
    /// A common class for scripts that handle UI Buttons
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class ButtonBase : MonoBehaviour {

        [SerializeField] protected bool debug;

        /// <summary>
        /// The Button component attached with this script. Initialized in Start.
        /// </summary>
        protected Button Button;
        /// <summary>
        /// The button's text, if any. Initialized in Start.
        /// </summary>
        [CanBeNull] protected TMP_Text ButtonText;

        /// <summary>
        /// Initializes common variables and listens for OnClick.
        /// If overriding, ensure base.Start() runs before anything else.
        /// </summary>
        protected virtual void Start() {
            Button = GetComponent<Button>();
            Button.onClick.AddListener(OnClick);
            ButtonText = GetComponentInChildren<TMP_Text>();
        }

        /// <summary>
        /// Event callback for when the Button is clicked.
        /// </summary>
        protected abstract void OnClick();
    }
}
