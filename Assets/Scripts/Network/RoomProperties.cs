using ExitGames.Client.Photon;
using Photon.Pun;

namespace Network {
    public class RoomProperties {

        private readonly Hashtable _changes = new();

        public int CurrentHole {
            get => (int)(PhotonNetwork.CurrentRoom.CustomProperties["CurrentHole"] ?? 0);
            set => SetProperty("CurrentHole", value);
        }

        /// <summary>
        /// Applies any changes made with this instance.
        /// </summary>
        public void ApplyChanges() {
            PhotonNetwork.CurrentRoom.SetCustomProperties(_changes);
            _changes.Clear();
        }

        private void SetProperty(string property, object value) {
            // TODO: Pretty sure only applying if MasterClient is wrong, test this
            if (PhotonNetwork.IsConnected /*&& PhotonNetwork.IsMasterClient*/) {
                _changes[property] = value;
            }
            PhotonNetwork.CurrentRoom.CustomProperties[property] = value;
        }
        
    }
}

