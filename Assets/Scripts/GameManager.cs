using System.Collections.Generic;
using DevLocker.Utils;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour {

    [SerializeField] private bool debug;

    public static GameManager Instance { get; private set; }
    private static readonly float TimeBeforeNextLevel = 3f;

    public Transform spawn;
    [SerializeField] private SceneReference nextLevel;

    private readonly HashSet<GameObject> _finishedPlayers = new();
    private float _nextMapTimer;
    private Logger _logger;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Cannot have multiple GameManager instances! This duplicate instance will be destroyed.");
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start() {
        _logger = new(this, debug);

        if (spawn == null) {
            spawn = new GameObject().transform;
            _logger.Warn("Spawn reference missing! It has been set to a new empty at "+spawn.position);
        }

        NetworkManager.onPlayerLeft += PlayerLeft;
    }

    private void Update() {
        if (_finishedPlayers.Count >= NetworkManager.PlayerCount) {
            if (_nextMapTimer >= TimeBeforeNextLevel) {
                _logger.Log("Timer finished. Loading next level...");
                _nextMapTimer = 0;
                foreach (var finishedPlayer in _finishedPlayers) {
                    // Rigidbody rb = finishedPlayer.GetComponent<Rigidbody>();
                    // rb.position = spawn.position + new Vector3(0, finishedPlayer.transform.localScale.y / 2, 0);
                    // rb.velocity = Vector3.zero;
                    string sceneName = nextLevel.SceneName;
                    Debug.Log(sceneName);
                    var scene = SceneManager.GetSceneByName(sceneName);
                    Debug.Log(scene);
                    PhotonNetwork.LoadLevel(sceneName);
                }
                _finishedPlayers.Clear();
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
    }
    
}
