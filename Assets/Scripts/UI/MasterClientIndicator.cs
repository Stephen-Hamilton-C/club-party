using System;
using Photon.Pun;
using UnityEngine;

public class MasterClientIndicator : MonoBehaviour {

    private PhotonView _view;

    private void Start() {
        _view = GetComponentInParent<PhotonView>();
    }

    private void Update() {
        gameObject.SetActive(_view.Owner.IsMasterClient);
    }
}
