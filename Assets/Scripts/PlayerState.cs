using System;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerState : MonoBehaviour {

    public delegate void StrokeEvent(bool canStroke);

    public static event StrokeEvent OnCanStrokeChanged;

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
    
    private void Awake() {
        _view = GetComponent<PhotonView>();

        if (_instance != null) {
            if (_view.IsMine) {
                Debug.Log("Multiple player characters owned by this player exist! The duplicate character will be destroyed.");
                PhotonNetwork.Destroy(gameObject);
            }
            Destroy(this);
        } else {
            _instance = this;
        }
    }

    private void OnDestroy() {
        if (_instance == this) {
            _instance = null;
        }
    }
}
