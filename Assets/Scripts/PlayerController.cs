using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {

    [SerializeField] private bool debug;
    
    [SerializeField] private float speed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float minSpeed = 0.1f;
    [SerializeField] private GameObject mouseIndicator;
    
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

    private void LateUpdate() {
        var currentPos = transform.position;
        var indicatorPos = PlayerState.MousePosition;
        indicatorPos.y = currentPos.y;

        mouseIndicator.transform.position = indicatorPos;
        mouseIndicator.transform.localPosition = Vector3.ClampMagnitude(mouseIndicator.transform.localPosition, maxSpeed);

        var line = mouseIndicator.GetComponent<LineRenderer>();
        line.SetPositions(new Vector3[] {
            currentPos,
            mouseIndicator.transform.position
        });
    }

    /// <summary>
    /// Called by PlayerInput
    /// </summary>
    [UsedImplicitly]
    public void OnClick() {
        _logger.Log("Registered player click");
        if (PlayerState.CanStroke) {
            _logger.Log("Player can stroke");
            Vector3 pointToBall = PlayerState.MousePosition - transform.position;
            pointToBall.y = 0;

            Vector3 force = Vector3.ClampMagnitude(-pointToBall * speed, maxSpeed);
            _logger.Log("Applying impulse: "+force);
            _rb.AddForce(force, ForceMode.Impulse);
        }
    }

}
