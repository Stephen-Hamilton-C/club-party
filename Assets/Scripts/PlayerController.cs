using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {

    [SerializeField] private LayerMask clickMask;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float minSpeed = 0.1f;
    
    private PhotonView _view;
    private Rigidbody _rb;
    private Camera _camera;

    private void Start() {
        _view = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
        
        // Don't let the player control other players
        if (!_view.IsMine)
            GetComponent<PlayerInput>().DeactivateInput();
    }

    private void FixedUpdate() {
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
        if (PlayerState.CanStroke) {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, clickMask.value,
                    QueryTriggerInteraction.Ignore)) {
                Vector3 pointToBall = hit.point - transform.position;
                pointToBall.y = 0;

                Vector3 force = Vector3.ClampMagnitude(-pointToBall * speed, maxSpeed);
                _rb.AddForce(force, ForceMode.Impulse);
            }
        }
    }

}
