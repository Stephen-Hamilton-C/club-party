using DevLocker.Utils;
using UnityEngine;

namespace SHamilton.ClubParty.UI.Vote {
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Course")]
    public class CourseData : ScriptableObject {

        public string courseName;
        public SceneReference courseScene;

    }
}