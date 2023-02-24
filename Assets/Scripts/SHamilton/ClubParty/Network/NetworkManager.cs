using System;
using DevLocker.Utils;
using ExitGames.Client.Photon;
using ParrelSync;
using Photon.Pun;
using Photon.Realtime;
using SHamilton.ClubParty.Serializer;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.Network {
    /// <summary>
    /// Provides an abstracted layer between code and the PhotonNetwork
    /// </summary>
    public class NetworkManager : MonoBehaviourPunCallbacks {

        [SerializeField] private bool debug;
        private static Logger _logger;

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

        #region PhotonNetwork Abstraction
        // Sometimes I need more control over what exactly happens with PhotonNetwork stuff.
        // Consistent use of the NetworkManager allows me to get that specific control without having to think about it.
        // The philosophy is that there should be no PhotonNetwork calls outside of the Network namespace
        
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
        /// Gets this client's Player instance
        /// </summary>
        public static Player LocalPlayer => PhotonNetwork.LocalPlayer;
        /// <summary>
        /// Gets the character associated with the current client
        /// </summary>
        public static GameObject LocalCharacter => LocalPlayer.GetCharacter();
        /// <summary>
        /// Gets this client's properties
        /// </summary>
        public static readonly PlayerProperties LocalPlayerProperties = new(LocalPlayer);
        /// <summary>
        /// Gets all currently connected players
        /// </summary>
        public static Player[] Players => PhotonNetwork.PlayerList;
        /// <summary>
        /// Gets all currently connected players except for this LocalPlayer
        /// </summary>
        public static Player[] OtherPlayers => PhotonNetwork.PlayerListOthers;
        /// <summary>
        /// Whether the client is the Master Client or not
        /// </summary>
        public static bool IsMasterClient => PhotonNetwork.IsMasterClient;
        /// <summary>
        /// Determines if the client is connected to a game
        /// </summary>
        public static bool IsConnected => PhotonNetwork.IsConnected;
        /// <summary>
        /// The Room the client is currently in. This is null if IsConnected is false.
        /// </summary>
        public static Room CurrentRoom => PhotonNetwork.CurrentRoom;
        /// <summary>
        /// Photon Network time, synced with the server
        /// </summary>
        public static double Time => PhotonNetwork.Time;

        /// <summary>
        /// Simulates network code without actually connecting to a server when enabled.
        /// Enabling this immediately starts the simulated connection.
        /// </summary>
        public static bool OfflineMode {
            get => PhotonNetwork.OfflineMode;
            set => PhotonNetwork.OfflineMode = value;
        }

        /// <summary>
        /// Network-Destroys the GameObject, unless it is static or not under this client's control.
        /// </summary>
        /// <param name="obj">The GameObject to Network-Destroy</param>
        public static void Destroy(GameObject obj) {
            PhotonNetwork.Destroy(obj);
        }

        /// <summary>
        /// Disconnects from any room and then the Photon servers
        /// </summary>
        public static void Disconnect() {
            PhotonNetwork.Disconnect();
        }

        /// <summary>
        /// Loads a scene across the network by scene index
        /// </summary>
        /// <param name="sceneIndex">The build index of the scene to load</param>
        public static void LoadLevel(int sceneIndex) {
            PhotonNetwork.LoadLevel(sceneIndex);
        }

        /// <summary>
        /// Loads a scene across the network by Scene object
        /// </summary>
        /// <param name="scene">The Scene to load</param>
        public static void LoadLevel(Scene scene) {
            PhotonNetwork.LoadLevel(scene.buildIndex);
        }

        /// <summary>
        /// Loads a scene across the network by SceneReference
        /// </summary>
        /// <param name="sceneReference">The SceneReference to load</param>
        public static void LoadLevel(SceneReference sceneReference) {
            PhotonNetwork.LoadLevel(sceneReference.SceneName);
        }

        /// <summary>
        /// Creates a networked GameObject
        /// </summary>
        /// <param name="prefabName">The name of the prefab. This prefab must be in Resources.</param>
        /// <param name="position">The position of the newly instantiated prefab.</param>
        /// <param name="rotation">The rotation of the newly instantiated prefab.</param>
        /// <param name="group"></param>
        /// <param name="data"></param>
        /// <returns>The newly instantiated prefab</returns>
        public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, byte group = 0,
            object[] data = null) {
            return PhotonNetwork.Instantiate(prefabName, position, rotation, group, data);
        }

        /// <summary>
        /// Creates a networked GameObject at position 0, 0, 0 and with default rotation.
        /// </summary>
        /// <param name="prefabName">The name of the prefab. This prefab must be in Resources.</param>
        /// <param name="group"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static GameObject Instantiate(string prefabName, byte group = 0, object[] data = null) {
            return PhotonNetwork.Instantiate(prefabName, Vector3.zero, Quaternion.identity, group, data);
        }

        /// <summary>
        /// Cancels any buffered RPC events on the given PhotonView if it is owned by this client.
        /// </summary>
        /// <param name="view">The PhotonView to clear buffered RPC events on.</param>
        public static void CleanRpcBufferIfMine(PhotonView view) {
            if (!view.IsMine) return;
            PhotonNetwork.CleanRpcBufferIfMine(view);
        }

        #endregion
        
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
            
            // Some properties not meant to be serialized will try to get serialized when connecting
            // Clear properties on disconnect to stop this
            LocalPlayerProperties.Clear();
            
            SceneManager.LoadScene(0);
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
            _logger.Log("Room properties updated");
            onRoomPropertiesChanged?.Invoke(propertiesThatChanged);
        }

        /// <summary>
        /// Connect to the master server
        /// </summary>
        /// <returns>Whether the connection was attempted or halted due to an error</returns>
        public static bool Connect() {
            _logger.Log("Connecting to master server...");
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
