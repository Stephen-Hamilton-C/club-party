using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Network {
    public class PlayerProperties {
        
        private readonly Player _player;
        private readonly Hashtable _changes = new();

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

        /// <summary>
        /// Sends the changes made by this instance to other clients.
        /// This expects that the player is currently connected to a room.
        /// </summary>
        public void ApplyChanges() {
            if (!PhotonNetwork.IsConnected) return;
            _player.SetCustomProperties(_changes);
            _changes.Clear();
        }
        
        public void Clear() {
            _player.CustomProperties = new Hashtable();
            _changes.Clear();
        }

        private void SetProperty(string property, object value) {
            if (PhotonNetwork.IsConnected) {
                _changes[property] = value;
            }
            _player.CustomProperties[property] = value;
        }

    }
}

