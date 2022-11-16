using UnityEngine;

public class OutOfBounds : MonoBehaviour {

    [SerializeField] private float minimumY = -25f;

    private Rigidbody _rb;
    private bool _useRigidbody;
    private Vector3 _origPosition;

    private void Start() {
        _useRigidbody = TryGetComponent(out _rb);
        _origPosition = transform.position;
    }

    private void LateUpdate() {
        if (_useRigidbody && _rb.position.y < minimumY) {
            _rb.velocity = Vector3.zero;
            _rb.position = _origPosition;
        } else if (transform.position.y < minimumY) {
            transform.position = _origPosition;
        }
    }
    
}
