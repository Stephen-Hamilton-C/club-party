using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
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

            var idleSprite = serializedObject.FindProperty("tabIdle").objectReferenceValue;
            var hoverSprite = serializedObject.FindProperty("tabHover").objectReferenceValue;
            var activeSprite = serializedObject.FindProperty("tabActive").objectReferenceValue;

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

                    var sprite = idleSprite;
                    if ((selectedTab == null && i == 0) || (selectedTab != null && button == selectedTab)) {
                        switch (_colorMode) {
                            case ColorModes.Active:
                                sprite = activeSprite;
                                break;
                            case ColorModes.Hover:
                                sprite = hoverSprite;
                                break;
                        }
                    }

                    button.background.sprite = (Sprite)sprite;
                }
            }

        }
    }
}

