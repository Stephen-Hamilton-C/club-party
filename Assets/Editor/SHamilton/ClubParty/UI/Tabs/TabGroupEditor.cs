using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI.Tabs {
    [CustomEditor(typeof(TabGroup))]
    public class TabGroupEditor : UnityEditor.Editor {

        enum ColorModes {
            Default,
            Hover,
            Active,
        }

        private ColorModes _colorMode = ColorModes.Default;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            var idleColor = serializedObject.FindProperty("tabIdle").colorValue;
            var hoverColor = serializedObject.FindProperty("tabHover").colorValue;
            var activeColor = serializedObject.FindProperty("tabActive").colorValue;

            var tabGroup = (TabGroup)target;
            var selectedTab = serializedObject.FindProperty("selectedTab").objectReferenceValue;
            var tabButtons = tabGroup.GetComponentsInChildren<TabButton>();

            if (!Application.isPlaying) {
                _colorMode = (ColorModes)EditorGUILayout.EnumPopup("Test Color Mode", _colorMode);
                for (int i = 0; i < tabButtons.Length; i++) {
                    var button = tabButtons[i];
                    if (button.background == null) {
                        button.background = button.GetComponent<Image>();
                    }

                    var color = idleColor;
                    if ((selectedTab == null && i == 0) || (selectedTab != null && button == selectedTab)) {
                        switch (_colorMode) {
                            case ColorModes.Active:
                                color = activeColor;
                                break;
                            case ColorModes.Hover:
                                color = hoverColor;
                                break;
                        }
                    }

                    button.background.color = color;
                }
            }

        }
    }
}

