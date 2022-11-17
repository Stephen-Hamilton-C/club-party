using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTracker : MonoBehaviour {

    public Transform player;
    [SerializeField] private float rotationSpeed = 7.5f;
    [SerializeField] private float trackSpeed = 10f;
    [SerializeField] private Vector3 positionOffset = Vector3.zero;
    [SerializeField] private Vector3 baseRotation = Vector3.zero;

    private CameraControls _controls;
    private InputAction _look;
    private InputAction _unlockCam;

    private void Awake() {
        _controls = new();
    }

    private void OnEnable() {
        _look = _controls.Player.Look;
        _look.Enable();
        _unlockCam = _controls.Player.CameraUnlock;
        _unlockCam.Enable();
    }

    private void OnDisable() {
        _look.Disable();
        _unlockCam.Disable();
    }

    private void Start() {
        NetworkManager.onJoinedRoom += () => {
            GameObject character = PhotonNetwork.LocalPlayer.CustomProperties["Character"] as GameObject;
            player = character?.transform;
        };
    }

    private void FixedUpdate() {
        if (player == null) return;

        Transform parent = transform.parent;
        parent.position = Vector3.Lerp(parent.position, player.position, Time.fixedDeltaTime * trackSpeed);
        
        transform.localPosition = positionOffset;
        transform.localRotation = Quaternion.Euler(baseRotation);
    }

    private void Update() {
        if (player == null) return;
        
        bool cameraUnlocked = _unlockCam.IsPressed();
        Cursor.visible = !cameraUnlocked;
        if (cameraUnlocked) {
            float mouseX = _look.ReadValue<Vector2>().x;
            transform.parent.Rotate(new Vector3(0, 1, 0), mouseX * rotationSpeed);
        }
    }

}
