using System;
using Ball;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
[RequireComponent(typeof(RawImage))]
    public class ReadyIndicator : MonoBehaviour {

        private RawImage _image;

        private void Start() {
            _image = GetComponent<RawImage>();
            
            UpdateColor(PlayerState.CanStroke);
            PlayerState.OnCanStrokeChanged += UpdateColor;
        }

        private void UpdateColor(bool canStroke) {
            _image.color = canStroke ? Color.green : Color.red;
        }

        private void OnDestroy() {
            PlayerState.OnCanStrokeChanged -= UpdateColor;
        }
    }
}
