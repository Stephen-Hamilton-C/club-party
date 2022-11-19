using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTracker : MonoBehaviour {

    [SerializeField] private bool debug;
    
    public Transform player;
    [SerializeField] private float rotationSpeed = 7.5f;
    [SerializeField] private float trackSpeed = 10f;
    [SerializeField] private Vector3 positionOffset = Vector3.zero;
    [SerializeField] private Vector3 baseRotation = Vector3.zero;
    [SerializeField] private float minPitchRotation;
    [SerializeField] private float maxPitchRotation;
    [SerializeField] private float zoom = 3;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float zoomStep = 0.5f;

    private CameraControls _controls;
    private InputAction _lookCtrl;
    private InputAction _unlockCamCtrl;
    private InputAction _zoomCtrl;

    private Transform _pivot;
    private float _pitch;
    private Logger _logger;

    private void Awake() {
        _controls = new();
    }

    private void OnEnable() {
        _lookCtrl = _controls.Player.Look;
        _lookCtrl.Enable();
        _unlockCamCtrl = _controls.Player.CameraUnlock;
        _unlockCamCtrl.Enable();
        _zoomCtrl = _controls.Player.Zoom;
        _zoomCtrl.Enable();
    }

    private void OnDisable() {
        _lookCtrl.Disable();
        _unlockCamCtrl.Disable();
        _zoomCtrl.Disable();
    }

    private void Start() {
        _logger = new(this, debug);
        NetworkManager.onJoinedRoom += () => {
            GameObject character = PhotonNetwork.LocalPlayer.CustomProperties["Character"] as GameObject;
            player = character?.transform;
            _logger.Log("Joined room. Character: "+character);
        };

        _pivot = transform.parent;
    }

    private void FixedUpdate() {
        if (player == null) return;

        _pivot.position = Vector3.Lerp(_pivot.position, player.position, Time.fixedUnscaledDeltaTime * trackSpeed);

        transform.localPosition = Vector3.Lerp(transform.localPosition, positionOffset * zoom, Time.fixedUnscaledDeltaTime * trackSpeed);
        transform.localRotation = Quaternion.Euler(baseRotation);
    }

    private void Update() {
        if (player == null) return;
        
        bool cameraUnlocked = _unlockCamCtrl.IsPressed();
        Cursor.visible = !cameraUnlocked;
        if (cameraUnlocked && _lookCtrl.triggered) {
            var mouse = _lookCtrl.ReadValue<Vector2>();
            var localRot = _pivot.localRotation.eulerAngles;
            
            // Calculate pitch
            var oldPitch = _pitch;
            _pitch = Mathf.Clamp(_pitch + mouse.y * rotationSpeed, minPitchRotation, maxPitchRotation);
            localRot.x -= _pitch - oldPitch;
            
            // Calculate yaw
            localRot.y += mouse.x * rotationSpeed;
            
            // Apply rotation
            _pivot.localRotation = Quaternion.Euler(localRot);
        }

        if (_zoomCtrl.triggered) {
            var zoomValue = _zoomCtrl.ReadValue<float>();
            if (zoomValue > 0) {
                zoom -= zoomStep;
            } else {
                zoom += zoomStep;
            }

            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        }
    }

}
