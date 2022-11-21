using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {

    [SerializeField] private bool debug;
    
    [SerializeField] private LayerMask clickMask;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float minSpeed = 0.1f;
    
    private PhotonView _view;
    private Rigidbody _rb;
    private Camera _camera;
    private Logger _logger;

    private void Start() {
        _logger = new(this, debug);
        _view = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
        
        // Don't let the player control other players
        _logger.Log("IsMine: "+_view.IsMine);
        if (!_view.IsMine)
            GetComponent<PlayerInput>().DeactivateInput();
    }

    private void FixedUpdate() {
        if (!_view.IsMine) return;
        
        PlayerState.CanStroke = _rb.velocity.magnitude <= minSpeed;
        if (_rb.velocity.magnitude < minSpeed) {
            _rb.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Called by PlayerInput
    /// </summary>
    [UsedImplicitly]
    public void OnClick() {
        _logger.Log("Registered player click");
        if (PlayerState.CanStroke) {
            _logger.Log("Player can stroke");
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, clickMask.value,
                    QueryTriggerInteraction.Ignore)) {
                _logger.Log("Click hit the control plane at "+hit.point);
                Vector3 pointToBall = hit.point - transform.position;
                pointToBall.y = 0;

                Vector3 force = Vector3.ClampMagnitude(-pointToBall * speed, maxSpeed);
                _logger.Log("Applying impulse: "+force);
                _rb.AddForce(force, ForceMode.Impulse);
            } else {
                _logger.Warn("Click missed the control plane");
            }
        }
    }

}
