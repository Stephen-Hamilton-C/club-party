using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Button))]
    public class MultiplayerButton : MonoBehaviour {
        [SerializeField] private bool debug;

        private Logger _logger;
        private Button _button;
        private TextMeshProUGUI _buttonText;

        private void Start() {
            _logger = new(this, debug);
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
            _buttonText = GetComponentInChildren<TextMeshProUGUI>();
            if (_buttonText == null) {
                _logger.Err("No TextMeshProUGUI found in children! This must have a text in its children!");
            }

            NetworkManager.onConnectedToMaster += JoinRoom;
            NetworkManager.onJoinedRoom += JoinedRoom;
        }

        private void OnClick() {
            bool willConnect = NetworkManager.Connect();
            _logger.Log("Connecting to Master Server...");
            _logger.Log(willConnect ? "Will connect..." : "Won't connect.");

            if (willConnect) {
                _button.interactable = false;
                _buttonText.text = "Connecting...";
            }
        }

        private void JoinRoom() {
            NetworkManager.JoinRoom();
        }

        private void JoinedRoom() {
            if (PhotonNetwork.IsMasterClient) {
                PhotonNetwork.LoadLevel(1);
            }
        }

        private void OnDestroy() {
            NetworkManager.onConnectedToMaster -= JoinRoom;
            NetworkManager.onJoinedRoom -= JoinedRoom;
        }
    }
}