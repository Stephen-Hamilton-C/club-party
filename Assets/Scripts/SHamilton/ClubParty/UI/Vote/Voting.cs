using System.Collections.Generic;
using System.Globalization;
using Photon.Pun;
using SHamilton.ClubParty.Network;
using SHamilton.Util;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Logger = SHamilton.Util.Logger;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEngine.SceneManagement;
#endif

// TODO: Both options are just "Course 1"
namespace SHamilton.ClubParty.UI.Vote {
    [RequireComponent(typeof(PhotonView))]
    public class Voting : MonoBehaviour, IPunObservable {
    
        [SerializeField] private bool debug;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private List<CourseData> courses = new();
        [SerializeField] private int countdownLength = 10;

        private Logger _logger;
        private PlaceholderReplacer _countdownReplacer;
        private PhotonView _view;
        private Toggle[] _toggles;

        private double _countdownStartTime = -1;
        private int[] _chosenCourses;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsReading) {
                _logger.Log("Reading!");
                _countdownStartTime = (double)stream.ReceiveNext();
                _chosenCourses = (int[])stream.ReceiveNext();
                UpdateButtons();
            } else if (stream.IsWriting) {
                _logger.Log("Writing!");
                stream.SendNext(_countdownStartTime);
                stream.SendNext(_chosenCourses);
            }
        }
	
        private void Start() {
            // Go back to main menu if this scene is loaded in editor
            #if UNITY_EDITOR
            if (!NetworkManager.IsConnected) {
                SceneManager.LoadScene(0);
            }
            #endif
            
            _logger = new(this, debug);
            _view = GetComponent<PhotonView>();
            _view.OwnershipTransfer = OwnershipOption.Takeover;
            _countdownReplacer = new PlaceholderReplacer(countdownText.text);
            Cursor.visible = true;
            
            // Reset vote
            NetworkManager.LocalPlayer.SetCurrentVote(-1);
            
            _toggles = toggleGroup.GetComponentsInChildren<Toggle>();
            foreach (var toggle in _toggles) {
                toggle.onValueChanged.AddListener((value) => { VoteChanged(toggle, value); });
            }
            
            if (NetworkManager.IsMasterClient) {
                _view.TransferOwnership(NetworkManager.LocalPlayer);
                _countdownStartTime = NetworkManager.Time;

                _logger.Log("Picking courses...");
                var courseIndices = new List<int>();
                for (int i = 0; i < courses.Count; i++) {
                    courseIndices.Add(i);
                }
                _logger.Log("Course indices: "+courseIndices.ToCommaSeparatedString());

                _chosenCourses = new int[toggleGroup.transform.childCount];
                for(int i = 0; i < _chosenCourses.Length; i++) {
                    var selectedIndex = Random.Range(0, courseIndices.Count);
                    _chosenCourses[i] = courseIndices[selectedIndex];
                    courseIndices.Remove(selectedIndex);
                    _logger.Log("Course selected: "+courses[selectedIndex].courseName);
                    _logger.Log("courseIndices: "+courseIndices.ToCommaSeparatedString());
                }
                
                UpdateButtons();
            }
        }

        private void UpdateButtons() {
            for (int i = 0; i < _toggles.Length; i++) {
                var text = _toggles[i].GetComponentInChildren<TMP_Text>();
                var courseIndex = _chosenCourses[i];
                text.text = courses[courseIndex].courseName;
            }
        }

        private void VoteChanged(Toggle toggle, bool value) {
            if (!value) return;
            var voteIndex = toggle.transform.GetSiblingIndex();
            NetworkManager.LocalPlayer.SetCurrentVote(voteIndex);

            var votedCourseIndex = _chosenCourses[voteIndex];
            var votedCourse = courses[votedCourseIndex];
            _logger.Log("Voted for "+votedCourse.courseName);
        }

        private void Update() {
            if (_countdownStartTime < 0) return;
            
            var timeSinceStart = NetworkManager.Time - _countdownStartTime;
            var timeUntilEnd = countdownLength - timeSinceStart;
            countdownText.text = _countdownReplacer
                .Replace("COUNTDOWN")
                .With(((int)timeUntilEnd).ToString(CultureInfo.CurrentCulture))
                .ReplacePlaceholders();

            if (NetworkManager.IsMasterClient && timeSinceStart >= countdownLength) {
                _countdownStartTime = -1;
                _logger.Log("Timer finished as master client.");
                var courseVotes = new int[_chosenCourses.Length];
                foreach (var player in NetworkManager.Players) {
                    var playerVote = player.GetCurrentVote();
                    if (playerVote < 0 || playerVote >= courseVotes.Length) continue;

                    courseVotes[playerVote]++;
                }

                var mostVotedIndex = 0;
                for (int i = 1; i < courseVotes.Length; i++) {
                    if (courseVotes[mostVotedIndex] < courseVotes[i]) {
                        mostVotedIndex = i;
                    }
                }

                var chosenCourseIndex = _chosenCourses[mostVotedIndex];
                var chosenCourse = courses[chosenCourseIndex];
                _logger.Log("Course chosen: " + chosenCourse.courseName);
                NetworkManager.LoadLevel(chosenCourse.courseScene);
            }
        }
    }
}

