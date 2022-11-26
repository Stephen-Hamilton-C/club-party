using System;
using UnityEngine;

namespace UI {
    /// <summary>
    /// Makes the transform on this component look at the main camera
    /// </summary>
    public class LookAtCamera : MonoBehaviour {

        private Transform _camTransform;

        private void Start() {
            _camTransform = Camera.main.transform;
        }

        private void LateUpdate() {
            Quaternion camRot = _camTransform.rotation;
            transform.LookAt(transform.position + camRot * Vector3.forward, camRot * Vector3.up);
        }
    }
}
