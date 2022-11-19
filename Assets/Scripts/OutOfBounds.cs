using UnityEngine;

public class OutOfBounds : MonoBehaviour {

    [SerializeField] private bool debug;
    [SerializeField] private float minimumY = -25f;

    private Rigidbody _rb;
    private bool _useRigidbody;
    private Vector3 _origPosition;
    private Logger _logger;

    public void ResetOrigPosition() {
        _origPosition = transform.position;
        _logger.Log("Reset original position to "+_origPosition);
    }

    private void Start() {
        _logger = new(this, debug);
        _useRigidbody = TryGetComponent(out _rb);
        ResetOrigPosition();
    }

    private void LateUpdate() {
        if (_useRigidbody && _rb.position.y < minimumY) {
            _rb.velocity = Vector3.zero;
            _rb.position = _origPosition;
            _logger.Log("GameObject fell below bounds. Resetting position to "+_origPosition+" using Rigidbody");
        } else if (transform.position.y < minimumY) {
            transform.position = _origPosition;
            _logger.Log("GameObject fell below bounds. Resetting position to "+_origPosition+" using Transform");
        }
    }
    
}
