using ExitGames.Client.Photon;
using Photon.Realtime;
using SHamilton.ClubParty.Network;
using SHamilton.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.Vote {
    [RequireComponent(typeof(Toggle))]
    [RequireComponent(typeof(Image))]
    public class VotingButton : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private Color selectedColor;

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
        private Color _deselectedColor;
        private TMP_Text _text;
        private PlaceholderReplacer _replacer;
	
        private void Start() {
            _logger = new(this, debug);
            toggle = GetComponent<Toggle>();
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<TMP_Text>();
            _replacer = new PlaceholderReplacer(_text.text);

            _deselectedColor = _image.color;
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

            _text.text = _replacer
                .Replace("NAME").With(_course.courseName)
                .Replace("C").With(voteCount)
                .ReplacePlaceholders();
        }

        private void ValueChanged(bool value) {
            var color = value ? selectedColor : _deselectedColor;
            _image.color = color;
            _logger.Log("Color set to "+color);
        }

        private void OnDestroy() {
            NetworkManager.onPlayerPropertiesChanged -= PlayerPropertiesChanged;
            NetworkManager.onPlayerLeft -= PlayerLeft;
        }
    }
}

