using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using SHamilton.ClubParty.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Logger = SHamilton.Util.Logger;
using Random = UnityEngine.Random;

// TODO: Redesign this system to utilize RoomProperties
namespace SHamilton.ClubParty.UI.Vote {
    [RequireComponent(typeof(PhotonView))]
    public class Voting : MonoBehaviour {

        [SerializeField] private bool debug;
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private int courseCount = 4;
        [SerializeField] private List<CourseData> allCourses = new();
        [SerializeField] private int countdownStart = 15;

        private Logger _logger;
        private PhotonView _view;
        private double _timer = Mathf.Infinity;
        private readonly Dictionary<string, TextMeshProUGUI> _texts = new();
        private readonly Dictionary<string, CourseData> _courses = new();
        private string _currentlyVotedCourse = "";
        private string _origCountdownText = "";
        
        private void Start() {
            _logger = new(this, debug);
            _view = GetComponent<PhotonView>();
            _view.OwnershipTransfer = OwnershipOption.Takeover;
            _origCountdownText = countdownText.text;
            Cursor.visible = true;
            
            // Reset vote
            NetworkManager.LocalPlayerProperties.CurrentVote = null;
            NetworkManager.LocalPlayerProperties.ApplyChanges();
            
            if (NetworkManager.IsMasterClient) {
                _view.TransferOwnership(NetworkManager.LocalPlayer);
                _view.RPC("StartCountdownRPC", RpcTarget.AllBuffered, NetworkManager.Time);
                
                _logger.Log("Picking next courses...");
                var coursesToPick = allCourses.ToList();
                var changedProperties = new Hashtable();
                for (int i = 0; i < courseCount; i++) {
                    var selectedIndex = Random.Range(0, coursesToPick.Count);
                    var selectedCourse = coursesToPick[selectedIndex];
                    changedProperties["VoteCourseName" + i] = selectedCourse.courseName;
                    changedProperties["VoteCount_" + selectedCourse.courseName] = 0;
                    coursesToPick.RemoveAt(selectedIndex);
                    _logger.Log("Course "+i+" selected: "+selectedCourse.courseName);
                }

                NetworkManager.CurrentRoom.SetCustomProperties(changedProperties);
            }

            NetworkManager.onRoomPropertiesChanged += RoomPropertiesChanged;
            NetworkManager.onPlayerLeft += RemovePlayerVote;
            RoomPropertiesChanged(NetworkManager.CurrentRoom.CustomProperties);
        }

        private void OnDestroy() {
            _logger.Log("Goodbye, world!");
            NetworkManager.CleanRpcBufferIfMine(_view);
            NetworkManager.onRoomPropertiesChanged -= RoomPropertiesChanged;
            NetworkManager.onPlayerLeft -= RemovePlayerVote;
        }

        private void VoteSelected(CourseData course) {
            if (_currentlyVotedCourse == course.courseName) return;
            _logger.Log("Voted for "+course.courseName);
            
            var changedProperties = new Hashtable();
            var currentProperties = NetworkManager.CurrentRoom.CustomProperties;
            if (_currentlyVotedCourse != "") {
                var oldCourseKey = "VoteCount_" + _currentlyVotedCourse;
                changedProperties[oldCourseKey] = (int)currentProperties[oldCourseKey] - 1;
                _logger.Log("Decremented count from previously voted course: "+_currentlyVotedCourse);
            }
            
            var newCourseKey = "VoteCount_" + course.courseName;
            if (currentProperties[newCourseKey] == null) {
                changedProperties[newCourseKey] = 1;
                _logger.Log("No previous vote count for course. Set to 1.");
            } else {
                changedProperties[newCourseKey] = (int)currentProperties[newCourseKey] + 1;
                _logger.Log("Set vote count to "+(int)changedProperties[newCourseKey]);
            }

            _currentlyVotedCourse = course.courseName;

            NetworkManager.CurrentRoom.SetCustomProperties(changedProperties);
            NetworkManager.LocalPlayerProperties.CurrentVote = _currentlyVotedCourse;
            NetworkManager.LocalPlayerProperties.ApplyChanges();
        }
        
        private void RoomPropertiesChanged(Hashtable changedProperties) {
            if (changedProperties.ContainsKey("VoteCourseName0")) {
                _logger.Log("Properties changed. Received course names.");
                
                // MasterClient decided on rooms
                var buttons = GetComponentsInChildren<Button>();
                for (int i = 0; i < courseCount; i++) {
                    var courseName = (string)changedProperties["VoteCourseName" + i];
                    var course = allCourses.Find(it => it.courseName == courseName);
                    _courses.Add(courseName, course);

                    var text = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
                    text.text = courseName + " (0)";
                    _texts.Add(courseName, text);
                    buttons[i].onClick.AddListener(() => VoteSelected(course));
                    _logger.Log("Added "+courseName);
                }
            }
            
            foreach (var (courseName, course) in _courses) {
                if (changedProperties.ContainsKey("VoteCount_" + courseName)) {
                    // Vote count has changed
                    _logger.Log("Vote count changed for "+courseName);
                    UpdateText(_texts[courseName], course);
                }
            }
        }

        private void RemovePlayerVote(Player player) {
            if (!NetworkManager.IsMasterClient) return;

            var playerProps = new PlayerProperties(player);
            if (playerProps.CurrentVote == null) return;
            var votedCourseName = playerProps.CurrentVote;
            var voteCountKey = "VoteCount_" + votedCourseName;
            var currentVoteCount = (int)NetworkManager.CurrentRoom.CustomProperties[voteCountKey];
            NetworkManager.CurrentRoom.SetCustomProperties(
                new Hashtable() { { voteCountKey, currentVoteCount - 1 } }
            );
        }

        private void UpdateText(TextMeshProUGUI text, CourseData course) {
            var voteCount =
                (int)NetworkManager.CurrentRoom.CustomProperties["VoteCount_" + course.courseName];
            text.text = course.courseName + " (" + voteCount + ")";
        }

        [PunRPC]
        [UsedImplicitly]
        private void StartCountdownRPC(double startTime) {
            var timeDifference = NetworkManager.Time - startTime;
            if (timeDifference > countdownStart * 10) {
                _logger.Err("NetworkManager.Time seems to have wrapped around. startTime: "+startTime+", NetworkManager.Time: "+NetworkManager.Time);
            }
            // TODO: Check if difference is larger than countdownStart * 2. If so, NetworkManager.Time wrapped around.
            // Will need to account for this edge case
            _timer = countdownStart - timeDifference;
            _logger.Log("RPC received to start countdown. startTime: "+startTime+". Timer set to "+_timer);
        }

        private void Update() {
            if (double.IsPositiveInfinity(_timer)) return;
            
            _timer -= Time.deltaTime;
            countdownText.text = _origCountdownText + " (" + (int)_timer + ")";
            
            if (NetworkManager.IsMasterClient && _timer < 0) {
                _timer = Mathf.Infinity;
                
                // Find course with most votes
                _logger.Log("Timer finished as master client.");
                CourseData chosenCourse = null;
                var mostVotes = Mathf.NegativeInfinity;
                var changedProperties = new Hashtable();
                foreach (var (courseName, course) in _courses) {
                    var votes = (int)NetworkManager.CurrentRoom.CustomProperties["VoteCount_" + courseName];
                    if (votes >= mostVotes) {
                        chosenCourse = course;
                        mostVotes = votes;
                    }

                    // Reset vote count
                    changedProperties["VoteCount_" + courseName] = 0;
                }

                _logger.Log("Most voted course: "+chosenCourse!.courseName);
                NetworkManager.CurrentRoom.SetCustomProperties(changedProperties);

                // Load first hole for course
                NetworkManager.LoadLevel(chosenCourse!.courseScene);
            }
        }
    }
}

