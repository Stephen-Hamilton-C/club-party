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
        #if UNITY_EDITOR
        PhotonNetwork.OfflineMode = offline;
        #endif

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
        
        _logger.Log("Connecting to Master Server...");
        _logger.Log(NetworkManager.Connect() ? "Will connect..." : "Won't connect.");
    }
    
}
