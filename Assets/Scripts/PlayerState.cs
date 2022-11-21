using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PhotonView))]
public class PlayerState : MonoBehaviour {

    [SerializeField] private bool debug;
    [SerializeField] private LayerMask clickMask;

    public delegate void StrokeEvent(bool canStroke);

    public static event StrokeEvent OnCanStrokeChanged;
    public static Vector3 MousePosition { get; private set; } = Vector3.zero;

    public static bool CanStroke {
        get => _canStroke;
        set {
            if (_canStroke != value) {
                _canStroke = value;
                if (OnCanStrokeChanged != null)
                    OnCanStrokeChanged(value);
            }
        }
    }

    private static bool _canStroke;
    
    private static PlayerState _instance;
    private PhotonView _view;
    private Camera _camera;
    private Vector2 _mouseScreenPos;
    private Logger _logger;
    
    private void Awake() {
        _logger = new(this, debug);
        _view = GetComponent<PhotonView>();
        _camera = Camera.main;

        if (!_view.IsMine) {
            _logger.Log("This is another player's character. Destroying this instance.");
            Destroy(this);
        }
        
        if (_instance != null) {
            if (_view.IsMine) {
                _logger.Warn("Multiple player characters owned by this player exist! The duplicate character will be destroyed.");
                PhotonNetwork.Destroy(gameObject);
            }
        } else {
            _instance = this;
        }
    }

    private void OnDestroy() {
        if (_instance == this) {
            _instance = null;
        }
    }

    private void OnMouseMove(InputValue mousePosValue) {
        _mouseScreenPos = mousePosValue.Get<Vector2>();
    }

    private void LateUpdate() {
        Ray ray = _camera.ScreenPointToRay(_mouseScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, clickMask.value, 
                    QueryTriggerInteraction.Ignore)) {
            PlayerState.MousePosition = hit.point;
        } else {
            _logger.Warn("Mouse raycast missed the control plane");
        }
    }

}
