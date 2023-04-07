using Photon.Pun;
using TMPro;
using UnityEngine;

namespace SHamilton.ClubParty.UI.Dev {
    public class PhotonStats : MonoBehaviour {

        [SerializeField] private TMP_Text connected;
        [SerializeField] private TMP_Text localPlayer;
        [SerializeField] private TMP_Text isMasterClient;
        [SerializeField] private TMP_Text masterClient;
        [SerializeField] private TMP_Text roomName;
        [SerializeField] private TMP_Text playerCount;
        [SerializeField] private TMP_Text photonTime;

        private void Start() {
            DontDestroyOnLoad(gameObject);
        }

        private void Update() {
            connected.text = "Connected: " + PhotonNetwork.IsConnectedAndReady;
            localPlayer.text = "LocalPlayer: " + PhotonNetwork.LocalPlayer;
            isMasterClient.text = "LocalPlayer is MasterClient: " + PhotonNetwork.IsMasterClient;
            masterClient.text = "MasterClient: " + PhotonNetwork.MasterClient;
            roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom?.Name;
            playerCount.text = "Player Count: " + PhotonNetwork.CurrentRoom?.PlayerCount;
            photonTime.text = "Photon Time: " + PhotonNetwork.Time;
        }
    }
}

