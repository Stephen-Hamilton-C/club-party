using System.Collections.Generic;
using DevLocker.Utils;
using Network;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public const string CharacterName = "Ball";

    public delegate void GameEvent();
    public delegate void PlayerEvent(GameObject playerObject);
    
    [SerializeField] private bool debug;

    public static GameManager Instance { get; private set; }
    private static readonly float TimeBeforeNextLevel = 3f;
    public static GameEvent OnHoleStarted;
    public static GameEvent OnHoleFinished;
    public static PlayerEvent OnPlayerFinished;

    public Transform spawn;
    [SerializeField] private SceneReference nextLevel;

    private readonly HashSet<GameObject> _finishedPlayers = new();
    private float _nextMapTimer;
    private Logger _logger;

    public static void StartGame() {
        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.LoadLevel(1);
        }
    }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Cannot have multiple GameManager instances! This duplicate instance will be destroyed.");
            Destroy(this);
            return;
        }

        Instance = this;
        
        // Spawn player
        var spawnPos = spawn.position;
        spawnPos.y += 0.1f;
        PhotonNetwork.Instantiate(CharacterName, spawnPos, spawn.rotation);
        // PlayerParenter will do the rest
        // TODO: Heesh that's a bad name
    }

    private void Start() {
        _logger = new(this, debug);

        if (spawn == null) {
            spawn = new GameObject().transform;
            _logger.Warn("Spawn reference missing! It has been set to a new empty at "+spawn.position);
        }

        NetworkManager.onPlayerLeft += PlayerLeft;

        if (OnHoleStarted != null)
            OnHoleStarted();
    }

    private void Update() {
        if (_finishedPlayers.Count >= NetworkManager.PlayerCount) {
            if (_nextMapTimer >= TimeBeforeNextLevel) {
                _logger.Log("Timer finished. Loading next level...");
                _nextMapTimer = 0;
                _finishedPlayers.Clear();

                if (OnHoleFinished != null)
                    OnHoleFinished();
                
                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
                if (PhotonNetwork.IsMasterClient) {
                    PhotonNetwork.LoadLevel(nextLevel.SceneName);
                }
            } else {
                _nextMapTimer += Time.unscaledDeltaTime;
            }
        }
    }

    private void OnDestroy() {
        _logger.Log("Goodbye, world!");
        if (Instance == this) {
            _logger.Log("The singleton instance is being destroyed.");
            Instance = null;
        }

        NetworkManager.onPlayerLeft -= PlayerLeft;
    }

    private void PlayerLeft(Player player) {
        // FIXME: Players are getting locked in levels when a player leaves
        GameObject character = PhotonNetwork.LocalPlayer.CustomProperties["Character"] as GameObject; 
        bool success = _finishedPlayers.Remove(character);
        _logger.Log("Player ("+player.NickName+") left. Character: "+character);
        if (success) {
            _logger.Log("Character removed from finished set.");
        } else {
            _logger.Log("Player was not in hole.");
        }
    }

    public void PlayerInHole(GameObject playerObject) {
        _logger.Log("Player has made it in the hole. playerObject: "+playerObject);
        _finishedPlayers.Add(playerObject);
        playerObject.layer = LayerMask.NameToLayer("PlayerNoCollide");
        if (OnPlayerFinished != null)
            OnPlayerFinished(playerObject);
    }
    
}
