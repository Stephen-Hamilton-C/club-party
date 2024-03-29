using Photon.Pun;
using Photon.Realtime;
using SHamilton.ClubParty.Network;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.Ball {
    /// <summary>
    /// Holds general information about the LocalPlayer
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(OutOfBounds))]
    public class LocalPlayerState : MonoBehaviour {

        [SerializeField] private bool debug;

        #region Events
        public delegate void TriggerEvent();
        public delegate void BoolEvent(bool canStroke);
        public static event BoolEvent OnCanStrokeChanged;
        public static event TriggerEvent OnStroke;
        public static event TriggerEvent OnStrokeFinished;
        public static event TriggerEvent OnHoleFinished;
        #endregion
    
        /// <summary>
        /// Determines if the player is able to stroke
        /// </summary>
        public static bool CanStroke {
            get => _canStroke;
            set {
                if (_canStroke != value) {
                    _canStroke = value;
                    OnCanStrokeChanged?.Invoke(value);
                }
            }
        }
        private static bool _canStroke;

        public static bool Stroked => _stroked;
        private static bool _stroked;
    
        private static LocalPlayerState _instance;
        private PhotonView _view;
        private OutOfBounds _outOfBounds;
        private Logger _logger;
    
        private void Awake() {
            _logger = new(this, debug);

            _view = GetComponent<PhotonView>();
            _outOfBounds = GetComponent<OutOfBounds>();
            
            // LocalPlayerState should only exist on the *local* player
            if (!_view.IsMine) {
                _logger.Log("This is another player's character. Destroying this instance.");
                Destroy(this);
            }
        
            
            // Ensure singleton
            if (_instance != null) {
                if (_view.IsMine) {
                    _logger.Warn("Multiple player characters owned by this player exist! The duplicate character will be destroyed.");
                    NetworkManager.Destroy(gameObject);
                }
            } else {
                _instance = this;
            }

            OnCanStrokeChanged += CanStrokeChanged;
            GameManager.OnPlayerFinished += PlayerFinished;
        }

        private void PlayerFinished(Player player) {
            if (player.IsLocal) {
                OnHoleFinished?.Invoke();
            }
        }

        private void OnDestroy() {
            // Release singleton
            if (_instance == this) {
                _instance = null;
            }

            OnCanStrokeChanged -= CanStrokeChanged;
            GameManager.OnPlayerFinished -= PlayerFinished;
        }

        private void CanStrokeChanged(bool value) {
            if (value && _stroked) {
                _stroked = false;
                _outOfBounds.SetRespawnPoint();
                OnStrokeFinished?.Invoke();
            }
        }

        /// <summary>
        /// Informs the LocalPlayerState that a stroke has been performed
        /// </summary>
        public static void Stroke() {
            _stroked = true;
            OnStroke?.Invoke();
        }
    }
}
