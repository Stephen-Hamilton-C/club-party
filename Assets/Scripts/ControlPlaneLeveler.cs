using Photon.Pun;
using UnityEngine;

public class ControlPlaneLeveler : MonoBehaviour {

    private void Update() {
        var character = (PhotonNetwork.LocalPlayer.CustomProperties["Character"] as GameObject)?.transform;
        if (character) {
            transform.position = new(0, character.position.y - character.localScale.y / 2, 0);
        }
    }
    
}
