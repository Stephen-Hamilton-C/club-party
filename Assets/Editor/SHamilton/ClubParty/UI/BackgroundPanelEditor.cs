using UnityEditor;
using UnityEditor.UI;

namespace SHamilton.ClubParty.UI {
    [CustomEditor(typeof(BackgroundPanel))]
    public class BackgroundPanelEditor : ImageEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
        
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("animate"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("time"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("delay"));

            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}

