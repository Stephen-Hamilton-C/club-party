using ExitGames.Client.Photon;
using Photon.Pun;

namespace Network {
    public class RoomProperties {

        private readonly Hashtable _changes = new();

        public int CurrentHole {
            get => (int)(NetworkManager.CurrentRoom.CustomProperties["CurrentHole"] ?? 0);
            set => SetProperty("CurrentHole", value);
        }

        /// <summary>
        /// Applies any changes made with this instance.
        /// </summary>
        public void ApplyChanges() {
            NetworkManager.CurrentRoom.SetCustomProperties(_changes);
            _changes.Clear();
        }

        private void SetProperty(string property, object value) {
            // TODO: Pretty sure only applying if MasterClient is wrong, test this
            if (NetworkManager.IsConnected /*&& NetworkManager.IsMasterClient*/) {
                _changes[property] = value;
            }
            NetworkManager.CurrentRoom.CustomProperties[property] = value;
        }
        
    }
}

