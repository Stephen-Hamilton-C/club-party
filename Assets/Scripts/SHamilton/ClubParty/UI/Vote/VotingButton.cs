using ExitGames.Client.Photon;
using Photon.Realtime;
using SHamilton.ClubParty.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.Vote {
    [RequireComponent(typeof(Toggle))]
    [RequireComponent(typeof(Image))]
    public class VotingButton : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private Sprite selectedSprite;

        public Toggle toggle;

        public CourseData Course {
            get => _course;
            set {
                _course = value;
                UpdateVoteCount();
            }
        }
        private CourseData _course;
        
        private Logger _logger;
        private Image _image;
        private Sprite _deselectedSprite;
        private TMP_Text _text;
	
        private void Awake() {
            _logger = new(this, debug);
            toggle = GetComponent<Toggle>();
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<TMP_Text>();

            _deselectedSprite = _image.sprite;
            toggle.onValueChanged.AddListener(ValueChanged);

            NetworkManager.onPlayerPropertiesChanged += PlayerPropertiesChanged;
            NetworkManager.onPlayerLeft += PlayerLeft;
            UpdateVoteCount();
        }

        private void PlayerPropertiesChanged(Player player, Hashtable changedProperties) {
            UpdateVoteCount();
        }

        private void PlayerLeft(Player player) {
            UpdateVoteCount();
        }

        private void UpdateVoteCount() {
            var siblingIndex = transform.GetSiblingIndex();
            var voteCount = 0;
            foreach (var player in NetworkManager.Players) {
                if (player.GetCurrentVote() == siblingIndex) {
                    voteCount++;
                }
            }

            if(_course)
                _text.text = _course.courseName + " ("+voteCount+")";
        }

        private void ValueChanged(bool value) {
            _image.sprite = value ? selectedSprite : _deselectedSprite;
        }

        private void OnDestroy() {
            NetworkManager.onPlayerPropertiesChanged -= PlayerPropertiesChanged;
            NetworkManager.onPlayerLeft -= PlayerLeft;
        }
    }
}

