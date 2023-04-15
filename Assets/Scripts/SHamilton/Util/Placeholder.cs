using System.Collections.Generic;
using System.Globalization;

namespace SHamilton.Util {
    public class Placeholder {
        /// <summary>
        /// The original text with its placeholders
        /// </summary>
        private readonly string _originalText;
        /// <summary>
        /// The text to replace each placeholder with
        /// </summary>
        private readonly Dictionary<string, string> _placeholderValues = new();
        /// <summary>
        /// The cached text that has been replaced
        /// </summary>
        private string _cachedText;
        /// <summary>
        /// Whether the cached text needs to be recalculated or not
        /// </summary>
        private bool _isCacheDirty;

        public Placeholder(string text) {
            _originalText = text;
        }

        /// <summary>
        /// Marks a placeholder to be replaced with the given text
        /// </summary>
        /// <param name="placeholderName">The placeholder to replace, without the % signs</param>
        /// <param name="replaceText">The text to replace the placeholder with</param>
        /// <returns>This Placeholder instance, to allow chained calls</returns>
        public Placeholder Set(string placeholderName, string replaceText) {
            _isCacheDirty = true;
            _placeholderValues.Add(placeholderName, replaceText);
            return this;
        }
        
        /// <summary>
        /// Marks a placeholder to be replaced with the given text
        /// </summary>
        /// <param name="placeholderName">The placeholder to replace, without the % signs</param>
        /// <param name="replaceInt">The number to replace the placeholder with</param>
        /// <returns>This Placeholder instance, to allow chained calls</returns>
        public Placeholder Set(string placeholderName, int replaceInt) {
            return Set(placeholderName, replaceInt.ToString());
        }
        
        /// <summary>
        /// Marks a placeholder to be replaced with the given text
        /// </summary>
        /// <param name="placeholderName">The placeholder to replace, without the % signs</param>
        /// <param name="replaceFloat">The number to replace the placeholder with</param>
        /// <returns>This Placeholder instance, to allow chained calls</returns>
        public Placeholder Set(string placeholderName, float replaceFloat) {
            return Set(placeholderName, replaceFloat.ToString(CultureInfo.CurrentCulture));
        }
        
        /// <summary>
        /// Marks a placeholder to be replaced with the given text
        /// </summary>
        /// <param name="placeholderName">The placeholder to replace, without the % signs</param>
        /// <param name="replaceDouble">The number to replace the placeholder with</param>
        /// <returns>This Placeholder instance, to allow chained calls</returns>
        public Placeholder Set(string placeholderName, double replaceDouble) {
            return Set(placeholderName, replaceDouble.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Replaces the placeholders with the given values from previously called Set functions.
        /// </summary>
        /// <returns>The text with its placeholders replaced</returns>
        public string Replace() {
            if (!_isCacheDirty) {
                return _cachedText;
            }
            
            var replaceText = _originalText;
            foreach (var (placeholder, value) in _placeholderValues) {
                replaceText = replaceText.Replace("%" + placeholder + "%", value);
            }

            _cachedText = replaceText;
            _isCacheDirty = false;

            return replaceText;
        }
    }
}
