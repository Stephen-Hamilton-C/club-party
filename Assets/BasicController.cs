using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
public class BasicController : MonoBehaviour {

    [SerializeField] private float speed = 5.0f;
    private Rigidbody rb;
    private PhotonView photon;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        photon = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!photon.IsMine) return;
        
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.AddForce(direction * speed);
    }
}
