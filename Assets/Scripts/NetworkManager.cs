using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks {

    [SerializeField] private bool debug;
    private Logger _logger;

    private static NetworkManager instance;
    
    public delegate void TriggerEvent();
    public delegate void DisconnectedEvent(DisconnectCause cause);
    public delegate void PlayerEvent(Player player);

    public static event TriggerEvent onConnectedToMaster;
    public static event TriggerEvent onJoinedRoom;
    public static event TriggerEvent onLeftRoom;
    public static event PlayerEvent onPlayerJoined;
    public static event PlayerEvent onPlayerLeft;
    public static event DisconnectedEvent onDisconnected;

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
        if (onConnectedToMaster != null)
            onConnectedToMaster();
    }

    public override void OnDisconnected(DisconnectCause cause) {
        _logger.Log("Disconnected: "+cause);
        if (onDisconnected != null)
            onDisconnected(cause);
    }

    public override void OnJoinedRoom() {
        _logger.Log("Joined room");
        if (onJoinedRoom != null)
            onJoinedRoom();
    }

    public override void OnLeftRoom() {
        _logger.Log("Left room");
        if (onLeftRoom != null)
            onLeftRoom();
    }

    public override void OnPlayerEnteredRoom(Player player) {
        _logger.Log("Player entered room. Name: "+player.NickName+", ActorNumber: "+player.ActorNumber);
        if (onPlayerJoined != null)
            onPlayerJoined(player);
    }

    public override void OnPlayerLeftRoom(Player player) {
        _logger.Log("Player left room. Name: "+player.NickName+", ActorNumber: "+player.ActorNumber);
        if (onPlayerLeft != null)
            onPlayerLeft(player);
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

    private void Awake() {
        if (instance != null) {
            Debug.LogError("Cannot have multiple NetworkManager instances! This duplicate instance will be destroyed.");
            Destroy(this);
            return;
        }

        instance = this;
        _logger = new(this, debug);
        
        PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName", "Player");
        PhotonNetwork.AutomaticallySyncScene = true;
        
        DontDestroyOnLoad(this);
    }

}
