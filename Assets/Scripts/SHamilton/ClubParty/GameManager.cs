using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using SHamilton.ClubParty.Network;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty {
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
        private const float TimeBeforeNextLevel = 1f;
    
        [SerializeField] private bool debug;

        #region Events
        public delegate void GameEvent();
        public delegate void PlayerEvent(Player player);
        public static GameManager Instance { get; private set; }
        /// <summary>
        /// Event when a player gets in a hole
        /// </summary>
        public static PlayerEvent OnPlayerFinished;
        /// <summary>
        /// Event when all players finish the level
        /// </summary>
        public static GameEvent OnHoleFinished;
        /// <summary>
        /// Event when all holes are finished
        /// </summary>
        public static GameEvent OnLevelFinished;
        #endregion

        /// <summary>
        /// Each hole in this course
        /// </summary>
        public Hole[] holes;
        public Hole CurrentHole => holes[HoleIndex];

        private static readonly RoomProperties RoomProps = new();
        public static int HoleIndex {
            get => RoomProps.CurrentHole;
            private set {
                RoomProps.CurrentHole = value;
                if(NetworkManager.IsMasterClient)
                    RoomProps.ApplyChanges();
            }
        }
    
        /// <summary>
        /// A Set of Players that finished the hole
        /// </summary>
        private readonly HashSet<Player> _finishedPlayers = new();
        /// <summary>
        /// Timer to track how long its been since all players made it into the hole
        /// </summary>
        private float _nextMapTimer;
        private Logger _logger;
        private Rigidbody _localCharRb;
        private Vector3 _spawnOffset;

        /// <summary>
        /// Loads the first level across the network if this player is the first to join
        /// </summary>
        public static void StartGame() {
            if (NetworkManager.IsMasterClient) {
                NetworkManager.LoadLevel(1);
                HoleIndex = 0;
            }
        }

        private void Awake() {
            _logger = new(this, debug);
        
            // Enforce singleton
            if (Instance != null) {
                Debug.LogError("Cannot have multiple GameManager instances! This duplicate instance will be destroyed.");
                Destroy(this);
                return;
            }
            Instance = this;

            if (holes.Length == 0)
                throw new InvalidOperationException("There are no spawns for this set!");
        
            // Spawn player
            NetworkManager.Instantiate(CharacterName);
            // PlayerSetup will do the rest
        }

        private void Start() {
            CurrentHole.isCurrent = true;

            var character = NetworkManager.LocalCharacter;
            _spawnOffset = new Vector3(0, character.transform.localScale.y / 2, 0);
            _localCharRb = character.GetComponent<Rigidbody>();
            // Must set transform position in Start because physics
            character.transform.position = CurrentHole.spawn.position + _spawnOffset;

            NetworkManager.onPlayerLeft += PlayerLeft;
        }

        private IEnumerator MovePlayer() {
            _localCharRb.isKinematic = true;
            yield return new WaitForFixedUpdate();
            _localCharRb.position = CurrentHole.spawn.position + _spawnOffset;
            yield return new WaitForFixedUpdate();
            _localCharRb.isKinematic = false;
        }
        
        private void HoleFinished() {
            _logger.Log("Hole finished. Old HoleIndex: "+HoleIndex+", new HoleIndex: "+(HoleIndex+1));
            CurrentHole.isCurrent = false;
            HoleIndex++;
            CurrentHole.isCurrent = true;
            StartCoroutine(MovePlayer());
            OnHoleFinished?.Invoke();
        }

        private void LevelFinished() {
            _logger.Log("Level finished.");
            OnLevelFinished?.Invoke();
        }

        private void Update() {
            if (_finishedPlayers.Count >= NetworkManager.PlayerCount) {
                // All players in hole. Wait for timer
                if (_nextMapTimer >= TimeBeforeNextLevel) {
                    _logger.Log("Timer finished. Loading next level...");
                    _nextMapTimer = 0;
                    _finishedPlayers.Clear();

                    if (HoleIndex + 1 < holes.Length) {
                        HoleFinished();
                    } else {
                        LevelFinished();
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
            var character = player.GetCharacter();
            character.layer = LayerMask.NameToLayer("PlayerNoCollide");
        
            // Event invoke
            OnPlayerFinished?.Invoke(player);
        }
    
    }
}
