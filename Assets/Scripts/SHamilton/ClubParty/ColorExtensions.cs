using System;
using UnityEngine;

namespace SHamilton.ClubParty {
    public static class ColorExtensions {
        public static string Serialize(this Color color) {
            return $"{color.r};{color.g};{color.b};{color.a}";
        }

        public static Color DeserializeColor(this string colorData) {
            var colorDataArray = colorData.Split(";");
            if (colorDataArray.Length != 4) {
                throw new InvalidOperationException("This string is not a serialized color!");
            }

            var r = float.Parse(colorDataArray[0]);
            var g = float.Parse(colorDataArray[1]);
            var b = float.Parse(colorDataArray[2]);
            var a = float.Parse(colorDataArray[3]);
            return new Color(r, g, b, a);
        }
    }
}