using ExitGames.Client.Photon;
using Photon.Realtime;
using SHamilton.ClubParty.Network;
using SHamilton.ClubParty.UI.Flair;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.Vote {
    [RequireComponent(typeof(Toggle))]
    [RequireComponent(typeof(SelectableColor))]
    public class VotingButton : MonoBehaviour {
    
        [SerializeField] private bool debug;

        public Toggle Toggle => _toggle;

        public CourseData Course {
            get => _course;
            set {
                _course = value;
                UpdateVoteCount();
            }
        }
        private CourseData _course;
        
        private Logger _logger;
        private Toggle _toggle;
        private SelectableColor _selectableColor;
        private TMP_Text _text;
	
        private void Awake() {
            _logger = new(this, debug);
            _toggle = GetComponent<Toggle>();
            _selectableColor = GetComponent<SelectableColor>();
            _text = GetComponentInChildren<TMP_Text>();

            Toggle.onValueChanged.AddListener(ValueChanged);

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
            _selectableColor.Color = value ? SelectableColor.Colors.Green : SelectableColor.Colors.Blue;
        }

        private void OnDestroy() {
            NetworkManager.onPlayerPropertiesChanged -= PlayerPropertiesChanged;
            NetworkManager.onPlayerLeft -= PlayerLeft;
        }
    }
}

