using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AutoJoin : MonoBehaviour {

    [SerializeField] private bool debug;
    [SerializeField] private GameObject player;
    private Logger _logger;

    private void Start() {
        _logger = new Logger(this, debug);
        Screen.fullScreen = false;

        NetworkManager.onConnectedToMaster += () => {
            _logger.Log("Connected to Master. Joining room...");
            NetworkManager.JoinRoom();
        };
        NetworkManager.onJoinedRoom += () => {
            _logger.Log("Room joined. Spawning player...");
            PhotonNetwork.Instantiate(player.name, new Vector3(0, player.transform.localScale.y / 2, 0),
                Quaternion.identity);
        };
        
        _logger.Log("Connecting to Master Server...");
        _logger.Log(NetworkManager.Connect() ? "Will connect..." : "Won't connect.");
    }
    
}
