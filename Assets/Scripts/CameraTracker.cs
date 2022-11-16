using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class CameraTracker : MonoBehaviour {

    public Transform player;
    [SerializeField] private float rotationSpeed = 7.5f;
    [SerializeField] private float trackSpeed = 10f;
    [SerializeField] private Vector3 positionOffset = Vector3.zero;
    [SerializeField] private Vector3 baseRotation = Vector3.zero;

    private void Start() {
        NetworkManager.onJoinedRoom += () => {
            GameObject character = PhotonNetwork.LocalPlayer.CustomProperties["Character"] as GameObject;
            player = character?.transform;
        };
    }

    private void FixedUpdate() {
        if (player == null) return;

        Transform parent = transform.parent;
        parent.position = Vector3.Lerp(parent.position, player.position, Time.fixedDeltaTime * trackSpeed);
        
        transform.localPosition = positionOffset;
        transform.localRotation = Quaternion.Euler(baseRotation);

        bool m2Held = Input.GetButton("Fire2");
        Cursor.visible = !m2Held;
        if (m2Held) {
            parent.Rotate(new Vector3(0, 1, 0), Input.GetAxis("Mouse X") * rotationSpeed);
        }
    }

}
