using Unity.VisualScripting;
using UnityEngine;

namespace SHamilton.ClubParty {
    public static class Prefs {
        public static Color CharacterColor {
            get {
                return GetColor("CharacterColor");
            }
            set {
                SetColor("CharacterColor", value);
            }
        }

        public static float MouseSensitivity {
            get {
                return PlayerPrefs.GetFloat("MouseSensitivity", 2.5f);
            }
            set {
                PlayerPrefs.SetFloat("MouseSensitivity", value);
            }
        }

        public static string PlayerName {
            get {
                return PlayerPrefs.GetString("PlayerName", "Player");
            }
            set {
                PlayerPrefs.SetString("PlayerName", value);
            }
        }

        private static Color GetColor(string key, string defaultValue = null) {
            if (defaultValue == null)
                defaultValue = Color.white.Serialize();
            return PlayerPrefs.GetString(key, defaultValue).DeserializeColor();
        }

        private static void SetColor(string key, Color value) {
            PlayerPrefs.SetString(key, value.Serialize());
        }
    }
}
