using System;
using Ball;
using ExitGames.Client.Photon;
using ParrelSync;
using Photon.Pun;
using Photon.Realtime;
using Serializer;
using UnityEngine;

namespace Network {
    public class NetworkManager : MonoBehaviourPunCallbacks {

        [SerializeField] private bool debug;
        private Logger _logger;

        private static NetworkManager _instance;
    
        public delegate void TriggerEvent();
        public delegate void DisconnectedEvent(DisconnectCause cause);
        public delegate void PlayerEvent(Player player);
        public delegate void PlayerPropertyEvent(Player player, Hashtable changedProperties);

        public static event TriggerEvent onConnectedToMaster;
        public static event TriggerEvent onJoinedRoom;
        public static event TriggerEvent onLeftRoom;
        public static event PlayerEvent onPlayerJoined;
        public static event PlayerEvent onPlayerLeft;
        public static event DisconnectedEvent onDisconnected;
        public static event PlayerPropertyEvent onPlayerPropertyChanged;
        public static event TriggerEvent onLocalCharacterInitialized;

        public static string PlayerName {
            get => PhotonNetwork.NickName;
            set {
                PlayerPrefs.SetString("PlayerName", value);
                PhotonNetwork.NickName = value;
            }
        }

        public static byte PlayerCount {
            get {
                if (PhotonNetwork.InRoom) {
                    return PhotonNetwork.CurrentRoom.PlayerCount;
                }

                return 0;
            }
        }

        public override void OnConnectedToMaster() {
            _logger.Log("Connected to Master Server");
            onConnectedToMaster?.Invoke();
        }

        public override void OnDisconnected(DisconnectCause cause) {
            _logger.Log("Disconnected: "+cause);
            onDisconnected?.Invoke(cause);
        }

        public override void OnJoinedRoom() {
            _logger.Log("Joined room");
            onJoinedRoom?.Invoke();
        }

        public override void OnLeftRoom() {
            _logger.Log("Left room");
            if (onLeftRoom != null)
                onLeftRoom();
        }

        public override void OnPlayerEnteredRoom(Player player) {
            _logger.Log("Player entered room. Name: "+player.NickName+", ActorNumber: "+player.ActorNumber);
            onPlayerJoined?.Invoke(player);
        }

        public override void OnPlayerLeftRoom(Player player) {
            _logger.Log("Player left room. Name: "+player.NickName+", ActorNumber: "+player.ActorNumber);
            onPlayerLeft?.Invoke(player);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {
            // Update local cache of PlayerProperties
            if (changedProps.TryGetValue("CharacterName", out var charNameRaw)) {
                var charName = (string)charNameRaw;
                var charPath = "/" + PlayerParenter.CharacterContainer.name + "/" + charName;
                var charObj = GameObject.Find(charPath);
                
                _logger.Log("CharacterName changed. Set "+targetPlayer+"'s cached Character to "+charObj);
                targetPlayer.CustomProperties["Character"] = charObj;
                if(targetPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    onLocalCharacterInitialized?.Invoke();
            }
            
            _logger.Log("Properties changed: "+changedProps);
            onPlayerPropertyChanged?.Invoke(targetPlayer, changedProps);
        }

        public static bool Connect() {
            PhotonNetwork.OfflineMode = false;
            return PhotonNetwork.ConnectUsingSettings();
        }

        public static bool ConnectAndJoinRoom() {
            onConnectedToMaster += JoinAfterConnect;
            return Connect();
        }

        private static void JoinAfterConnect() {
            JoinRoom();
            onConnectedToMaster -= JoinAfterConnect;
        }

        public static void ConnectOffline() {
            PhotonNetwork.OfflineMode = true;
        }

        public static void ConnectOfflineAndJoinRoom() {
            onConnectedToMaster += JoinAfterConnect;
            PhotonNetwork.OfflineMode = true;
        }

        public static bool JoinRoom() {
            return PhotonNetwork.JoinRandomOrCreateRoom();
        }

        public static bool LeaveRoom() {
            return PhotonNetwork.LeaveRoom(false);
        }

        public static void SetPlayerProperty(object key, object value) {
            var table = new Hashtable() {{key, value}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(table);
        }

        private void RegisterSerializers() {
            ColorSerializer.Register();
            AudioClipSerializer.Register();
        }
    
        private void Awake() {
            if (_instance != null) {
                Debug.LogError("Cannot have multiple NetworkManager instances! This duplicate instance will be destroyed.");
                Destroy(this);
                return;
            }

            _instance = this;
            _logger = new(this, debug);

            RegisterSerializers();
        
            PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName", "Player");
            PhotonNetwork.AutomaticallySyncScene = true;
        
            DontDestroyOnLoad(this);
        
#if UNITY_EDITOR
            PhotonNetwork.GameVersion += "-DEV";

            if (ClonesManager.IsClone()) {
                var projectName = ClonesManager.GetCurrentProject().name;
                var suffixIndex = projectName.IndexOf(ClonesManager.CloneNameSuffix, StringComparison.Ordinal);
                var suffix = projectName.Substring(suffixIndex);

                PhotonNetwork.NickName += suffix;
            }
#endif
        }

    }
}
