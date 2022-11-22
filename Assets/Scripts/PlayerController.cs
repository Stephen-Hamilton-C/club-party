using System;
using System.Numerics;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {

    [SerializeField] private bool debug;
    
    [SerializeField] private float speed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float minSpeed = 0.1f;
    [SerializeField] private Transform mouseTarget;
    
    private PhotonView _view;
    private Rigidbody _rb;
    private Camera _camera;
    private CameraControls _controls;
    private InputAction _unlockCamCtrl;
    private Logger _logger;

    private void Awake() {
        _controls = new();
    }

    private void OnEnable() {
        _unlockCamCtrl = _controls.Player.CameraUnlock;
        _unlockCamCtrl.Enable();
    }

    private void OnDisable() {
        _unlockCamCtrl.Disable();
    }

    private void Start() {
        _logger = new(this, debug);
        _view = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
        
        // Don't let the player control other players
        _logger.Log("IsMine: "+_view.IsMine);
        if (!_view.IsMine) {
            GetComponent<PlayerInput>().DeactivateInput();
            Destroy(mouseTarget.gameObject);
        }
    }

    private void FixedUpdate() {
        if (!_view.IsMine) return;
        
        // Calculate 
        PlayerState.CanStroke = _rb.velocity.magnitude <= minSpeed;
        if (PlayerState.CanStroke) {
            _rb.velocity = Vector3.zero;
        }
    }

    private void LateUpdate() {
        if (!_view.IsMine) return;

        if (_unlockCamCtrl.IsPressed() || !PlayerState.CanStroke) {
            mouseTarget.gameObject.SetActive(false);
        } else {
            mouseTarget.gameObject.SetActive(true);
            
            // Calculate mouseTarget position
            var currentPos = transform.position;
            var mousePos = PlayerState.MousePosition;
            mousePos.y = currentPos.y;

            var mouseToBall = mousePos - currentPos;
            if (mouseToBall.magnitude > maxSpeed) {
                // Mouse is beyond maximum limit, we must clamp
                // First, calculate the largest vector from ball to mouse
                var clampedOutward = Vector3.ClampMagnitude(currentPos - mousePos, maxSpeed);
                // Find the amount of excess from mouse to ball
                // these are in opposite directions, so addition makes it smaller
                var excess = mouseToBall + clampedOutward;
                // Move along the mouseToBall vector by the excess amount
                var clampedMousePos = Vector3.MoveTowards(mousePos, currentPos, excess.magnitude);
                mouseTarget.position = clampedMousePos;
            } else {
                // Mouse is within range, make no further calculation
                mouseTarget.position = mousePos;
            }
        }
    }

    /// <summary>
    /// Called by PlayerInput
    /// </summary>
    [UsedImplicitly]
    public void OnClick() {
        _logger.Log("Registered player click");
        if (PlayerState.CanStroke) {
            Vector3 pointToBall = mouseTarget.position - transform.position;
            pointToBall.y = 0;

            Vector3 force = -pointToBall * speed;
            _logger.Log("Applying impulse: "+force);
            _rb.AddForce(force, ForceMode.Impulse);
        }
    }

}
