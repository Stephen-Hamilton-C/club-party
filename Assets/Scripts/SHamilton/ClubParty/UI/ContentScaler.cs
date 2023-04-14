using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI {
    [ExecuteInEditMode]
    public class ContentScaler : MonoBehaviour {
        private List<RectTransform> _children = new();
        private RectTransform _rectTransform;
        [CanBeNull] private LayoutGroup _layoutGroup;

        private void Start() {
            _rectTransform = GetComponent<RectTransform>();
            _layoutGroup = GetComponent<LayoutGroup>();
        }

        private void UpdateChildren() {
            _children.Clear();
            foreach (var childTransform in transform) {
                if (childTransform is RectTransform rectChild && rectChild.gameObject.activeSelf) {
                    _children.Add(rectChild);
                }
            }
        }

        private void Update() {
            UpdateChildren();
            
            var height = 0f;
            foreach (var child in _children) {
                height += child.rect.height;
            }
            
            if (_layoutGroup) {
                if (_layoutGroup is HorizontalOrVerticalLayoutGroup horvGroup) {
                    // Add spacing * _children.Count
                    height += horvGroup.spacing * _children.Count;
                }

                // Add top and bottom padding
                height += _layoutGroup.padding.top;
                height += _layoutGroup.padding.bottom;
            }

            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
    }
}
