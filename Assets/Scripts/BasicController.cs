using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
public class BasicController : MonoBehaviour {

    [SerializeField] private float speed = 500.0f;
    private Rigidbody _rb;
    private PhotonView _view;
    private Vector3 _direction;

    // Start is called before the first frame update
    void Start() {
        _rb = GetComponent<Rigidbody>();
        _view = GetComponent<PhotonView>();
    }

    void Update() {
        if (!_view.IsMine) return;
        
        if (Input.GetButtonDown("Right"))
            _direction.x += 1;
        if (Input.GetButtonDown("Left"))
            _direction.x -= 1;
        if (Input.GetButtonDown("Up"))
            _direction.z += 1;
        if (Input.GetButtonDown("Down"))
            _direction.z -= 1;
    }
    
    // Update is called once per frame
    void FixedUpdate() {
        if (!_view.IsMine) return;

        if (_direction.magnitude > 0) {
            _rb.AddForce(_direction * speed);
            _direction = Vector3.zero;
        }
    }
}
