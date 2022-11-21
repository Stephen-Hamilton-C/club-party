using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FOVMatcher : MonoBehaviour {
    
    private Camera _camera;
    private Camera _parentCamera;
	
    private void Start() {
        _camera = GetComponent<Camera>();
        _parentCamera = GetComponentInParent<Camera>();
    }

    private void LateUpdate() {
        _camera.fieldOfView = _parentCamera.fieldOfView;
    }
    
}
