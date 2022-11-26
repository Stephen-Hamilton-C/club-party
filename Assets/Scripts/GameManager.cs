using System.Collections.Generic;
using DevLocker.Utils;
using Network;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

/// <summary>
/// Handles level management
/// </summary>
public class GameManager : MonoBehaviour {

    /// <summary>
    /// The name of the player prefab
    /// </summary>
    private const string CharacterName = "Ball";
    /// <summary>
    /// How much time to wait before loading the next level
    /// </summary>
    private const float TimeBeforeNextLevel = 2f;

    
    [SerializeField] private bool debug;

    #region Events
    public delegate void GameEvent();
    public delegate void PlayerEvent(Player player);
    public static GameManager Instance { get; private set; }
    /// <summary>
    /// Event when a player finishes the level
    /// </summary>
    public static PlayerEvent OnPlayerFinished;
    #endregion

    /// <summary>
    /// The initial spawn point of the map 
    /// </summary>
    [SerializeField] private Transform spawn;
    /// <summary>
    /// The scene for the next hole.
    /// If this is null, this is the last hole for the set.
    /// </summary>
    [SerializeField] private SceneReference nextLevel;

    /// <summary>
    /// A Set of Players that finished the hole
    /// </summary>
    private readonly HashSet<Player> _finishedPlayers = new();
    /// <summary>
    /// Timer to track how long its been since all players made it into the hole
    /// </summary>
    private float _nextMapTimer;
    private Logger _logger;

    /// <summary>
    /// Loads the first level across the network if this player is the first to join
    /// </summary>
    public static void StartGame() {
        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.LoadLevel(1);
        }
    }

    private void Awake() {
        // Enforce singleton
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

        // Fallback spawn point if dev forgets to set spawn
        if (spawn == null) {
            spawn = new GameObject().transform;
            _logger.Warn("Spawn reference missing! It has been set to a new empty at "+spawn.position);
        }

        NetworkManager.onPlayerLeft += PlayerLeft;
    }

    private void Update() {
        if (_finishedPlayers.Count >= NetworkManager.PlayerCount) {
            // All players in hole. Wait for timer
            if (_nextMapTimer >= TimeBeforeNextLevel) {
                _logger.Log("Timer finished. Loading next level...");
                _nextMapTimer = 0;
                _finishedPlayers.Clear();

                // If we don't destroy all player objects, then they somehow get duplicated when other players join
                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
                if (PhotonNetwork.IsMasterClient) {
                    PhotonNetwork.LoadLevel(nextLevel.SceneName);
                }
            } else {
                _nextMapTimer += Time.unscaledDeltaTime;
            }
        } else {
            _nextMapTimer = 0;
        }
    }

    private void OnDestroy() {
        // Remove singleton reference in case this is the result of a new scene load
        _logger.Log("Goodbye, world!");
        if (Instance == this) {
            _logger.Log("The singleton instance is being destroyed.");
            Instance = null;
        }
        
        NetworkManager.onPlayerLeft -= PlayerLeft;
    }

    private void PlayerLeft(Player player) {
        // TODO: Players are getting locked in levels when a player leaves
        // Is PhotonNetwork.PlayerCount being updated?
        // Maybe it should be room...
        bool success = _finishedPlayers.Remove(player);
        _logger.Log("Player ("+player.NickName+") left.");
        _logger.Log(success ? "Player removed from finished set." : "Player was not in hole.");
    }

    /// <summary>
    /// Notifies the GameManager that a player has made it into the hole
    /// </summary>
    /// <param name="player">The ActorNumber of the PhotonPlayer that finished the hole</param>
    public void PlayerInHole(Player player) {
        _logger.Log("Player has made it in the hole. Player: "+player);
        _finishedPlayers.Add(player);
        
        // Disable player inter-collision
        var character = player.CustomProperties["Character"] as GameObject;
        character.layer = LayerMask.NameToLayer("PlayerNoCollide");
        
        // Event invoke
        OnPlayerFinished?.Invoke(player);
    }
    
}
