using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Serializer;
using UnityEngine;

#if UNITY_EDITOR
using ParrelSync;
#endif

namespace Network {
    /// <summary>
    /// Provides an abstracted layer between code and the PhotonNetwork
    /// </summary>
    public class NetworkManager : MonoBehaviourPunCallbacks {

        [SerializeField] private bool debug;
        private Logger _logger;

        private static NetworkManager _instance;

        #region Events
        public delegate void TriggerEvent();
        public delegate void DisconnectedEvent(DisconnectCause cause);
        public delegate void CustomPropertyEvent(Hashtable changedProperties);
        public delegate void PlayerEvent(Player player);

        public static event TriggerEvent onConnectedToMaster;
        public static event TriggerEvent onJoinedRoom;
        public static event TriggerEvent onLeftRoom;
        public static event PlayerEvent onPlayerJoined;
        public static event PlayerEvent onPlayerLeft;
        public static event DisconnectedEvent onDisconnected;
        public static event CustomPropertyEvent onRoomPropertiesChanged;
        #endregion

        /// <summary>
        /// The name of the local player
        /// </summary>
        public static string PlayerName {
            get => PhotonNetwork.NickName;
            set {
                PlayerPrefs.SetString("PlayerName", value);
                PhotonNetwork.NickName = value;
            }
        }

        /// <summary>
        /// The number of players in the current room. Returns 0 if not in room.
        /// </summary>
        public static byte PlayerCount => PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.PlayerCount : (byte)0;

        /// <summary>
        /// Pun callback when connected to master server
        /// </summary>
        public override void OnConnectedToMaster() {
            _logger.Log("Connected to Master Server");
            onConnectedToMaster?.Invoke();
        }

        /// <summary>
        /// Pun callback when disconnected
        /// </summary>
        /// <param name="cause">The reason for the disconnect</param>
        public override void OnDisconnected(DisconnectCause cause) {
            _logger.Log("Disconnected: "+cause);
            onDisconnected?.Invoke(cause);
        }

        /// <summary>
        /// Pun callback when local player joins a room
        /// </summary>
        public override void OnJoinedRoom() {
            _logger.Log("Joined room");
            onJoinedRoom?.Invoke();
        }

        /// <summary>
        /// Pun callback when local player leaves a room
        /// </summary>
        public override void OnLeftRoom() {
            _logger.Log("Left room");
            onLeftRoom?.Invoke();
        }

        /// <summary>
        /// Pun callback when another player enters the current room
        /// </summary>
        /// <param name="player">The player that joined</param>
        public override void OnPlayerEnteredRoom(Player player) {
            _logger.Log("Player entered room. Name: "+player.NickName+", ActorNumber: "+player.ActorNumber);
            onPlayerJoined?.Invoke(player);
        }

        /// <summary>
        /// Pun callback when another player leaves the current room
        /// </summary>
        /// <param name="player">The player that left</param>
        public override void OnPlayerLeftRoom(Player player) {
            _logger.Log("Player left room. Name: "+player.NickName+", ActorNumber: "+player.ActorNumber);
            onPlayerLeft?.Invoke(player);
        }

        /// <summary>
        /// Pun callback when the custom properties of the room changed
        /// </summary>
        /// <param name="propertiesThatChanged">The properties that changed</param>
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
            onRoomPropertiesChanged?.Invoke(propertiesThatChanged);
        }

        /// <summary>
        /// Connect to the master server
        /// </summary>
        /// <returns>Whether the connection was attempted or halted due to an error</returns>
        public static bool Connect() {
            PhotonNetwork.OfflineMode = false;
            return PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// Connects to the master server and then immediately joins a room
        /// </summary>
        /// <returns>Whether the connection was attempted or halted due to an error</returns>
        public static bool ConnectAndJoinRoom() {
            onConnectedToMaster += JoinAfterConnect;
            return Connect();
        }

        /// <summary>
        /// Joins a room and removes this method from onConnectedToMaster
        /// </summary>
        private static void JoinAfterConnect() {
            JoinRoom();
            onConnectedToMaster -= JoinAfterConnect;
        }

        /// <summary>
        /// Starts offline mode
        /// </summary>
        public static void ConnectOffline() {
            PhotonNetwork.OfflineMode = true;
        }

        /// <summary>
        /// Starts offline mode and immediately joins a room
        /// </summary>
        public static void ConnectOfflineAndJoinRoom() {
            onConnectedToMaster += JoinAfterConnect;
            PhotonNetwork.OfflineMode = true;
        }

        /// <summary>
        /// Joins a room or creates one if one does not exist
        /// </summary>
        /// <returns>If an attempt will be made to join</returns>
        public static bool JoinRoom() {
            return PhotonNetwork.JoinRandomOrCreateRoom();
        }

        /// <summary>
        /// Leaves the current room
        /// </summary>
        /// <returns>If the current room could be left</returns>
        public static bool LeaveRoom() {
            return PhotonNetwork.LeaveRoom(false);
        }

        /// <summary>
        /// Registers all custom types with Photon
        /// </summary>
        private void RegisterSerializers() {
            ColorSerializer.Register();
            AudioClipSerializer.Register();
        }
    
        private void Awake() {
            // Singleton
            if (_instance != null) {
                Debug.LogWarning("Cannot have multiple NetworkManager instances! This duplicate instance will be destroyed.");
                Destroy(this);
                return;
            }
            _instance = this;
            
            _logger = new(this, debug);

            RegisterSerializers();
        
            // Load name and initialize settings
            PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName", "Player");
            PhotonNetwork.AutomaticallySyncScene = true;
        
            DontDestroyOnLoad(this);
        
#if UNITY_EDITOR
            // Editor shouldn't join standalone clients - that'll cause all sorts of mess
            PhotonNetwork.GameVersion += "-DEV";

            if (ClonesManager.IsClone()) {
                // Add suffix to player name
                var projectName = ClonesManager.GetCurrentProject().name;
                var suffixIndex = projectName.IndexOf(ClonesManager.CloneNameSuffix, StringComparison.Ordinal);
                var suffix = projectName.Substring(suffixIndex);

                PhotonNetwork.NickName += suffix;
            }
#endif
        }

    }
}
