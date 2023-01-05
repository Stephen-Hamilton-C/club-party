using ExitGames.Client.Photon;
using Photon.Pun;

namespace Network {
    public static class RoomProperties {

        public static int CurrentHole {
            get => (int)(PhotonNetwork.CurrentRoom.CustomProperties["CurrentHole"] ?? 0);
            set => SetProperty("CurrentHole", value);
        }
        
        

        private static void SetProperty(string property, object value) {
            if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient) {
                var hashtable = new Hashtable() { {property, value} };
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            }
            PhotonNetwork.CurrentRoom.CustomProperties[property] = value;
        }
        
    }
}

