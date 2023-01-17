using System;
using Network;
using Photon.Pun;
using UnityEngine;

/// <summary>
/// Simply keeps the control plane at the same y level as the player
/// </summary>
public class ControlPlaneLeveler : MonoBehaviour {

    private Transform _character;

    private void Start() {
        _character = NetworkManager.LocalCharacter.transform;
    }

    private void Update() {
        if (!_character) return;
        transform.position = new(0, _character.position.y - _character.localScale.y / 2, 0);
    }

}
