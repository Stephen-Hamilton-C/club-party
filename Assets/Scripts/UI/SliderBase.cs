using System.Globalization;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    /// <summary>
    /// A common class for scripts that handle UI Sliders
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public abstract class SliderBase : MonoBehaviour {

        [SerializeField] protected bool debug;
        [Tooltip("Determines how to round the manual input display. 0.01 rounds to the nearest hundredths place (0.00).")]
        [SerializeField] private float roundTo = 0.01f;

        /// <summary>
        /// The Slider component attached with this script. Initialized in Start.
        /// </summary>
        protected Slider Slider;
        /// <summary>
        /// The slider's label, if any. Initialized in Start.
        /// </summary>
        [CanBeNull] protected TMP_Text SliderLabel;
        /// <summary>
        /// An optional input field inside the slider to provide the user with precise control.
        /// </summary>
        [CanBeNull] protected TMP_InputField SliderInput;

        private Logger _logger;

        /// <summary>
        /// Initializes common variables and listens for onValueChanged.
        /// If overriding, ensure base.Start() runs before anything else.
        /// </summary>
        protected virtual void Start() {
            _logger = new(this, debug);
            
            Slider = GetComponent<Slider>();
            Slider.onValueChanged.AddListener(OnValueChanged);
            SliderLabel = GetComponentInChildren<TMP_Text>();
            SliderInput = GetComponentInChildren<TMP_InputField>();
            if (SliderInput != null) {
                Slider.onValueChanged.AddListener(UpdateManualInput);
                SliderInput.onEndEdit.AddListener(UpdateSlider);
                SliderInput.contentType = Slider.wholeNumbers ? TMP_InputField.ContentType.IntegerNumber : TMP_InputField.ContentType.DecimalNumber;
                _logger.Log("A manual input was found inside the slider. Based on slider settings, contentType has been set to "+SliderInput.contentType);
                UpdateManualInput(Slider.value);
            }
        }

        /// <summary>
        /// Event callback for when the Slider's value is changed.
        /// </summary>
        protected abstract void OnValueChanged(float value);

        /// <summary>
        /// Rounds the decimal given to the value set by roundTo
        /// </summary>
        /// <param name="value">The value to round</param>
        /// <returns>A decimal number rounded to value set by roundTo</returns>
        private float RoundValue(float value) {
            return Mathf.RoundToInt(value / roundTo) * roundTo;
        }

        /// <summary>
        /// Updates the manual input with the sliders value.
        /// Intended to be called on slider value change.
        /// </summary>
        /// <param name="value">The value of the slider</param>
        private void UpdateManualInput(float value) {
            if (SliderInput == null) return;
            
            var roundedValue = RoundValue(value);
            // This shouldn't result in an infinite loop because onEndEdit doesn't get triggered by text being set
            SliderInput.text = roundedValue.ToString(CultureInfo.InvariantCulture);
            _logger.Log("Slider value changed. Set manual input to "+SliderInput.text);
        }

        /// <summary>
        /// Updates the slider value with the manual input.
        /// Intended to be called on manual input end edit.
        /// </summary>
        /// <param name="rawValue">The text the user entered into the manual input</param>
        private void UpdateSlider(string rawValue) {
            if (SliderInput == null) return;
            
            if (float.TryParse(rawValue, out var value)) {
                // This shouldn't result in an infinite loop. See UpdateManualInput for details.
                Slider.value = value;
                _logger.Log("Manual input changed. Set slider value to "+Slider.value);
            } else {
                // Invalid value
                _logger.Log("Unable to parse user input as float.");
            }
            
            SliderInput.text = RoundValue(Slider.value).ToString(CultureInfo.InvariantCulture);
        }
    }
}
