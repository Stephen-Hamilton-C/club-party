using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace SHamilton.Util {
    public class PlaceholderReplacer {

        private readonly Dictionary<string, (int, int)> _placeholderRanges = new();
        private readonly Dictionary<string, string> _placeholderValues;
        private readonly string _text = "";
        [CanBeNull] private string _replaceString = null;

        public PlaceholderReplacer(string text, Dictionary<string, string> placeholderValues = null) {
            _text = text;
            if (placeholderValues == null) {
                _placeholderValues = new();
            } else {
                _placeholderValues = placeholderValues;
            }
            
            ParsePlaceholders();
        }

        public PlaceholderReplacer Replace(string placeholder) {
            _replaceString = placeholder;
            return this;
        }

        public PlaceholderReplacer With(string value) {
            if (_replaceString == null) {
                throw new InvalidOperationException("Tried to run With(string) before running Replace(string)!");
            }
            _placeholderValues[_replaceString] = value;
            _replaceString = null;
            return this;
        }

        private void ParsePlaceholders() {
            _placeholderRanges.Clear();
            
            var nextPlaceholder = "";
            var inPlaceholder = false;
            var startIndex = 0;
            for(int i = 0; i < _text.Length; i++) {
                var character = _text[i];
                if (character == '\\' && i+1 < _text.Length && _text[i+1] == '%') {
                    nextPlaceholder += "%";
                    i++;
                    continue;
                }

                if (character == '%') {
                    if(!inPlaceholder) {
                        inPlaceholder = true;
                        startIndex = i;
                    } else if (inPlaceholder) {
                        inPlaceholder = false;
                        _placeholderRanges.Add(nextPlaceholder, (startIndex, i));
                        nextPlaceholder = "";
                    }
                } else if (inPlaceholder) {
                    nextPlaceholder += character;
                }
            }
        }

        public string ReplacePlaceholders() {
            var replacedText = _text;
            foreach (var (placeHolderName, value) in _placeholderValues) {
                var placeholderExists = _placeholderRanges.TryGetValue(placeHolderName, out var ranges);
                if (!placeholderExists) {
                    LogWarn("Wanted to replace %"+placeHolderName+"%, but that placeholder was not found in the text!");
                    continue;
                }

                replacedText = replacedText.Substring(0, ranges.Item1) +
                               value +
                               replacedText.Substring(ranges.Item2);
                
                // Placeholder indices have moved, re-parse them
                ParsePlaceholders();
            }

            if (_placeholderRanges.Count > 0) {
                LogWarn("Not all placeholders have been replaced! " +
                        _placeholderRanges.Count+" placeholder(s) remain.");
            }
            
            return replacedText;
        }

        private void LogWarn(string msg) {
            Debug.LogWarning("<b>[PlaceholderReplacer]</b>: " + msg);
        }
    }
}

