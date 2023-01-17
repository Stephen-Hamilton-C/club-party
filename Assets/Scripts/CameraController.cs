using Network;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles all player control over the camera and its positioning around the player character
/// </summary>
public class CameraController : MonoBehaviour {

    [SerializeField] private bool debug;
    
    [Tooltip("The mouse sensitivity of the camera rotation")]
    [SerializeField] private float rotationSpeed = 7.5f;
    [Tooltip("How quickly the camera keeps up with the player")]
    [SerializeField] private float trackSpeed = 10f;
    [Tooltip("The position of the camera relative to the player")]
    [SerializeField] private Vector3 positionOffset = Vector3.zero;
    [Tooltip("The rotation of the camera so it looks at the player")]
    [SerializeField] private Vector3 baseRotation = Vector3.zero;
    [Tooltip("The minimum pitch rotation of the camera")]
    [SerializeField] private float minPitchRotation;
    [Tooltip("The maximum pitch rotation of the camera")]
    [SerializeField] private float maxPitchRotation;
    [Tooltip("The starting zoom (distance away from player) of the camera")]
    [SerializeField] private float zoom = 1;
    [Tooltip("How close the camera can get to the player")]
    [SerializeField] private float minZoom;
    [Tooltip("How far away the camera can go from the player")]
    [SerializeField] private float maxZoom;
    [Tooltip("How much the zoom should change on a single tick of the zoom control")]
    [SerializeField] private float zoomStep = 0.5f;

    #region Inputs
    /// <summary>
    /// The axes to use for looking around
    /// </summary>
    private InputAction _lookCtrl;
    /// <summary>
    /// The button to use to unlock the camera's rotation
    /// </summary>
    private InputAction _unlockCamCtrl;
    /// <summary>
    /// The axis to use for zooming
    /// </summary>
    private InputAction _zoomCtrl;
    #endregion

    private Logger _logger;
    /// <summary>
    /// The transform to use when rotating the camera
    /// </summary>
    private Transform _pivot;
    /// <summary>
    /// The player character's transform
    /// </summary>
    private Transform _player;
    /// <summary>
    /// The current pitch of the camera
    /// </summary>
    private float _pitch;

    private void Awake() {
        // Init inputs
        CameraControls controls = new();
        _lookCtrl = controls.Player.Look;
        _unlockCamCtrl = controls.Player.CameraUnlock;
        _zoomCtrl = controls.Player.Zoom;
    }

    private void OnEnable() {
        // Enable inputs
        _lookCtrl.Enable();
        _unlockCamCtrl.Enable();
        _zoomCtrl.Enable();
    }

    private void OnDisable() {
        // Disable inputs
        _lookCtrl.Disable();
        _unlockCamCtrl.Disable();
        _zoomCtrl.Disable();
    }

    private void Start() {
        _logger = new(this, debug);
        _pivot = transform.parent;
        
        // Get player's character from NetworkManager
        var character = NetworkManager.LocalCharacter;
        _player = character.transform;
        _logger.Log("Character: "+character);
    }

    private void FixedUpdate() {
        if (!_player) return;

        // Lerp camera to player's position
        _pivot.position = Vector3.Lerp(_pivot.position, _player.position, Time.fixedUnscaledDeltaTime * trackSpeed);

        // Lerp camera zoom
        transform.localPosition = Vector3.Lerp(transform.localPosition, positionOffset * zoom, Time.fixedUnscaledDeltaTime * trackSpeed);
        // Make camera look at player
        transform.localRotation = Quaternion.Euler(baseRotation);
    }

    private void Update() {
        if (!_player) return;
        
        // Rotation control
        bool cameraUnlocked = _unlockCamCtrl.IsPressed();
        Cursor.visible = !cameraUnlocked;
        if (cameraUnlocked && _lookCtrl.triggered) {
            var mouse = _lookCtrl.ReadValue<Vector2>();
            var localRot = _pivot.localRotation.eulerAngles;
            var sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2.5f);
            
            // Calculate pitch
            var oldPitch = _pitch;
            _pitch = Mathf.Clamp(_pitch + mouse.y * rotationSpeed * sensitivity, minPitchRotation, maxPitchRotation);
            localRot.x -= _pitch - oldPitch;
            
            // Calculate yaw
            localRot.y += mouse.x * rotationSpeed * sensitivity;
            
            // Apply rotation
            _pivot.localRotation = Quaternion.Euler(localRot);
        }

        // Zoom control
        if (_zoomCtrl.triggered) {
            // We only care about zoom direction
            var zoomValue = _zoomCtrl.ReadValue<float>();
            if (zoomValue > 0)
                zoom -= zoomStep;
            else
                zoom += zoomStep;

            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        }
    }

}
