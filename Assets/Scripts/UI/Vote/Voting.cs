using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Network;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace UI.Vote {
    [RequireComponent(typeof(PhotonView))]
    public class Voting : MonoBehaviour, IPunObservable {

        [SerializeField] private int courseCount = 4;
        [SerializeField] private List<CourseData> allCourses = new();
        [SerializeField] private bool debug;
        [SerializeField] private int countdown = 30;

        private Logger _logger;
        private PhotonView _view;
        private int _timer;
        private readonly Dictionary<string, TextMeshProUGUI> _texts = new();
        private readonly Dictionary<string, CourseData> _courses = new();
        private string _currentlyVotedCourse = "";
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsReading) {
                _timer = (int)stream.ReceiveNext();
            } else if(stream.IsWriting) {
                stream.SendNext(_timer);
            }
        }
        
        private void Start() {
            _logger = new(this, debug);
            _view = GetComponent<PhotonView>();
            _view.OwnershipTransfer = OwnershipOption.Takeover;
            if (PhotonNetwork.IsMasterClient) {
                _view.TransferOwnership(PhotonNetwork.LocalPlayer);
                StartCoroutine(Countdown());
                
                var coursesToPick = allCourses.ToList();
                var changedProperties = new Hashtable();
                for (int i = 0; i < courseCount; i++) {
                    var selectedIndex = Random.Range(0, coursesToPick.Count);
                    var selectedCourse = coursesToPick[selectedIndex];
                    changedProperties["VoteCourseName" + i] = selectedCourse.courseName;
                    coursesToPick.RemoveAt(selectedIndex);
                }

                PhotonNetwork.CurrentRoom.SetCustomProperties(changedProperties);
            }
            
            NetworkManager.onRoomPropertiesChanged += RoomPropertiesChanged;
        }

        private void VoteSelected(CourseData course) {
            var changedProperties = new Hashtable();
            var currentProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            if (!_currentlyVotedCourse.IsNullOrEmpty()) {
                var oldCourseKey = "VoteCount_" + _currentlyVotedCourse;
                changedProperties[oldCourseKey] = (int)currentProperties[oldCourseKey] - 1;
            }

            var newCourseKey = "VoteCount_" + course.courseName;
            changedProperties[newCourseKey] = (int)changedProperties[newCourseKey] + 1;
            _currentlyVotedCourse = course.courseName;

            PhotonNetwork.CurrentRoom.SetCustomProperties(changedProperties);
        }

        private void RoomPropertiesChanged(Hashtable changedProperties) {
            if (changedProperties.ContainsKey("VoteCourseName0")) {
                // MasterClient decided on rooms
                var buttons = GetComponentsInChildren<Button>();
                for (int i = 0; i < courseCount; i++) {
                    var courseName = (string)changedProperties["VoteCourseName" + i];
                    var course = allCourses.Find(it => it.courseName == courseName);
                    _courses.Add(courseName, course);

                    var text = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
                    _texts.Add(courseName, text);
                    // TODO: Assign button listeners to VoteSelected
                }
            }
            
            foreach (var (courseName, course) in _courses) {
                if (changedProperties.ContainsKey("VoteCount_" + courseName)) {
                    // Vote count has changed
                    UpdateText(_texts[courseName], course);
                }
            }
        }

        private void UpdateText(TextMeshProUGUI text, CourseData course) {
            var voteCount =
                (int)PhotonNetwork.CurrentRoom.CustomProperties["VoteCount_" + course.courseName];
            text.text = course.courseName + " (" + voteCount + ")";
        }

        private IEnumerator Countdown() {
            _timer = countdown;
            for (int i = 0; i < countdown; i++) {
                _timer--;
                yield return new WaitForSeconds(1);
            }
            
            
        }

    }
}

