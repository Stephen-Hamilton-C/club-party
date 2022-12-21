using DevLocker.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Vote {
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Course")]
    public class CourseData : ScriptableObject {

        public string courseName;
        public SceneReference courseScene;

    }
}