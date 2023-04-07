using UnityEngine;

namespace SHamilton.ClubParty {
    public static class StringExtensions {
        public static void CopyToClipboard(this string str) {
            GUIUtility.systemCopyBuffer = str;
        }
    }
}