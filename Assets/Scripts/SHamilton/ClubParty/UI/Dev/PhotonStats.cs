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
        [SerializeField] private double photonTimeRound = 0.0001;

        private static PhotonStats _instance;

        private void Start() {
            if (_instance == null) {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                // Player must have come back to the menu.
                // Don't need several instances of these.
                Destroy(gameObject);
            }
        }

        private double Round(double number, double place) {
            var num = (int)(number / place);
            return num * place;
        }
        
        private void Update() {
            connected.text = "Connected: " + PhotonNetwork.IsConnectedAndReady;
            localPlayer.text = "LocalPlayer: " + PhotonNetwork.LocalPlayer;
            isMasterClient.text = "LocalPlayer is MasterClient: " + PhotonNetwork.IsMasterClient;
            masterClient.text = "MasterClient: " + PhotonNetwork.MasterClient;
            roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom?.Name;
            playerCount.text = "Player Count: " + PhotonNetwork.CurrentRoom?.PlayerCount;
            photonTime.text = "Photon Time: " + Round(PhotonNetwork.Time, photonTimeRound);
        }
    }
}

