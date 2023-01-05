using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Network {
    public class PlayerProperties {
        
        private readonly Player _player;

        public PlayerProperties(Player player) {
            _player = player;
        }

        public GameObject Character {
            get => (GameObject)_player.CustomProperties["Character"];
            set => _player.CustomProperties["Character"] = value;
        }

        public Color CharacterColor {
            get => (Color)_player.CustomProperties["CharacterColor"];
            set => SetProperty("CharacterColor", value);
        }

        public string CurrentVote {
            get => (string)_player.CustomProperties["CurrentVote"];
            set => SetProperty("CurrentVote", value);
        }

        public int[] Scores {
            get => (int[])_player.CustomProperties["Scores"];
            set => SetProperty("Scores", value);
        }
        
        public void Clear() {
            _player.CustomProperties = new Hashtable();
        }

        private void SetProperty(string property, object value) {
            if (PhotonNetwork.IsConnected) {
                var hashtable = new Hashtable() { { property, value } };
                _player.SetCustomProperties(hashtable);
            }
            _player.CustomProperties[property] = value;
        }

    }
}

