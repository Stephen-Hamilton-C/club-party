using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    [SerializeField] private LayerMask clickMask;
    [SerializeField] private float speed = 50f;
    [SerializeField] private float maxSpeed = 100f;
    
    private PhotonView _view;
    private Rigidbody _rb;
    private Camera _camera;

    private void Start() {
        _view = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
    }

    private void Update() {
        if (!_view.IsMine) return;

        if (Input.GetButtonDown("Fire1")) {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickMask.value, QueryTriggerInteraction.Ignore)) {
                Vector3 pointToBall = hit.point - transform.position;
                pointToBall.y = 0;
                
                Vector3 force = Vector3.ClampMagnitude(-pointToBall * speed, maxSpeed);
                _rb.AddForce(force, ForceMode.Impulse);
            }
        }
    }
    
}
