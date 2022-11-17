using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AutoJoin : MonoBehaviour {

    [SerializeField] private bool debug;
    [SerializeField] private bool offline;
    [SerializeField] private GameObject player;
    private Logger _logger;

    private void Awake() {
        _logger = new Logger(this, debug);
        Screen.fullScreen = false;

        NetworkManager.onConnectedToMaster += () => {
            _logger.Log("Connected to Master. Joining room...");
            NetworkManager.JoinRoom();
        };
        NetworkManager.onJoinedRoom += () => {
            _logger.Log("Room joined. Spawning player...");
            GameObject character = PhotonNetwork.Instantiate(player.name, new Vector3(0, player.transform.localScale.y / 2, 0),
                Quaternion.identity);
            
            PhotonNetwork.LocalPlayer.CustomProperties.Add("Character", character);
        };
        
        if (Application.isEditor && offline) {
            PhotonNetwork.OfflineMode = true;
            _logger.Log("Offline mode activated.");
            _logger.Log("Joining an offline room...");
            NetworkManager.LeaveRoom();
            NetworkManager.JoinRoom();
        } else {
            _logger.Log("Connecting to Master Server...");
            _logger.Log(NetworkManager.Connect() ? "Will connect..." : "Won't connect.");
        }
    }
    
}
