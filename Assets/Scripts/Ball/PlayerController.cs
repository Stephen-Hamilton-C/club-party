using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ball {
    /// <summary>
    /// Handles controlling the player's ball
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour {

        [SerializeField] private bool debug;
    
        [Tooltip("Force multiplier to apply when the ball is hit")]
        public float speed = 2.5f;
        [Tooltip("The maximum distance the player can pull the mouse back")]
        public float maxPullDistance = 2f;
        [Tooltip("The minimum magnitude of the ball before it is locked to 0 and control to the player is restored")]
        public float cutOffVelocity = 0.25f;
        [Tooltip("The GameObject to use to represent the mouse position")]
        [SerializeField] private Transform mouseTarget;
        [Tooltip("How quickly the mouse target keeps up with the mouse position")]
        [SerializeField] private float mouseTargetSpeed = 10f;

        #region Input
        private CameraControls _controls;
        private InputAction _unlockCamCtrl;
        #endregion
    
        private PhotonView _view;
        private Rigidbody _rb;
        private Camera _camera;
        /// <summary>
        /// The screen position of the mouse
        /// </summary>
        private Vector3 _mouseScreenPos;
        /// <summary>
        /// The position of the mouse on the control plane
        /// </summary>
        private Vector3 _mousePos;
        private Logger _logger;
        /// <summary>
        /// Whether the player has clicked to prepare a stroke
        /// </summary>
        private bool _aiming;

        private void Awake() {
            _controls = new();
            _unlockCamCtrl = _controls.Player.CameraUnlock;
        }

        private void OnEnable() {
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
            if (_view.IsMine) {
                GameManager.OnPlayerFinished += PlayerFinishedHole;
            } else {
                GetComponent<PlayerInput>().DeactivateInput();
                Destroy(mouseTarget.gameObject);
            }
        }

        private void OnDestroy() {
            GameManager.OnPlayerFinished -= PlayerFinishedHole;
        }

        private void FixedUpdate() {
            if (!_view.IsMine) return;
        
            // Update CanStroke and force the ball to stop if below cutOffVelocity
            LocalPlayerState.CanStroke = _rb.velocity.magnitude <= cutOffVelocity;
            if (LocalPlayerState.CanStroke)
                _rb.velocity = Vector3.zero;
        }

        private void LateUpdate() {
            if (!_view.IsMine) return;
            
            // Update mouse position on control plane
            var ray = _camera.ScreenPointToRay(_mouseScreenPos);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity,
                    LayerMask.GetMask("Control"), QueryTriggerInteraction.Ignore)) {
                _mousePos = hit.point;
            }

            if (_unlockCamCtrl.IsPressed() || !LocalPlayerState.CanStroke || !_aiming) {
                // TODO: Are the extra OR checks necessary?
                // Player isn't aiming, ensure the indicator is hidden
                _aiming = false;
                mouseTarget.gameObject.SetActive(false);
                mouseTarget.position = transform.position;
            } else if(_aiming) {
                // Player is aiming, enable indicator
                mouseTarget.gameObject.SetActive(true);
            
                // Calculate mouse position
                var currentPos = transform.position;
                _mousePos.y = currentPos.y;

                Vector3 desiredTargetPosition;
                var mouseToBall = _mousePos - currentPos;
                if (mouseToBall.magnitude > maxPullDistance) {
                    // Mouse is beyond maximum limit, we must clamp
                    // First, calculate the largest vector from ball to mouse
                    var clampedOutward = Vector3.ClampMagnitude(currentPos - _mousePos, maxPullDistance);
                    // Find the amount of excess from mouse to ball
                    // these are in opposite directions, so addition makes it smaller
                    var excess = mouseToBall + clampedOutward;
                    // Move along the mouseToBall vector by the excess amount
                    var clampedMousePos = Vector3.MoveTowards(_mousePos, currentPos, excess.magnitude);
                    desiredTargetPosition = clampedMousePos;
                } else {
                    // Mouse is within range, make no further calculation
                    desiredTargetPosition = _mousePos;
                }

                // Lerp position for smoothness
                mouseTarget.position = Vector3.Lerp(mouseTarget.position, desiredTargetPosition, Time.deltaTime * mouseTargetSpeed);
            }
        }

        /// <summary>
        /// Updates the _mouseScreenPos with the current screen position of the mouse
        /// </summary>
        [UsedImplicitly]
        private void OnMouseMove(InputValue mousePosValue) {
            _mouseScreenPos = mousePosValue.Get<Vector2>();
        }
        
        /// <summary>
        /// Called by PlayerInput
        /// </summary>
        [UsedImplicitly]
        public void OnClick() {
            _logger.Log("Registered player click");
            if (_aiming && LocalPlayerState.CanStroke) {
                _aiming = false;
                
                // Get vector to mouse position on control plane
                var pointToBall = mouseTarget.position - transform.position;
                pointToBall.y = 0;

                // Apply force
                var force = -pointToBall * speed;
                _logger.Log("Applying impulse: "+force);
                _rb.AddForce(force, ForceMode.Impulse);
                LocalPlayerState.Stroked();
            } else if (!_aiming) {
                // Toggle aiming on
                _aiming = LocalPlayerState.CanStroke && !_unlockCamCtrl.IsPressed();
            }
        }

        /// <summary>
        /// Called by PlayerInput
        /// </summary>
        [UsedImplicitly]
        public void OnCancelShot() {
            _logger.Log("Shot canceled");
            _aiming = false;
        }

        /// <summary>
        /// Called by GameManager when a player finishes a hole
        /// </summary>
        /// <param name="player">The player that finished</param>
        private void PlayerFinishedHole(Player player) {
            if (PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber) {
                // Player is LocalPlayer. Hide target and destroy controller.
                // TODO: This should be a boolean switch for future plans
                GameManager.OnPlayerFinished -= PlayerFinishedHole;
                mouseTarget.gameObject.SetActive(false);
                Destroy(this);
            }
        }

    }
}
