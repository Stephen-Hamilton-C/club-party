using ExitGames.Client.Photon;
using UnityEngine;

namespace SHamilton.ClubParty.Serializer {
    /// <summary>
    /// Provides methods to Photon to serialize and deserialize a UnityEngine.Color
    /// </summary>
    public static class ColorSerializer {

        /// <summary>
        /// Converts a UnityEngine.Color to 4 floats representing red, green, blue, and alpha in that order
        /// </summary>
        /// <returns>The length of the byte array containing the serialized data</returns>
        private static short Serialize(StreamBuffer outStream, object customObject) {
            var color = (Color)customObject;
            var memColor = new byte[4 * 4];
            var i = 0;
            
            Protocol.Serialize(color.r, memColor, ref i);
            Protocol.Serialize(color.g, memColor, ref i);
            Protocol.Serialize(color.b, memColor, ref i);
            Protocol.Serialize(color.a, memColor, ref i);
            outStream.Write(memColor, 0, memColor.Length);
            
            return (short)memColor.Length;
        }

        /// <summary>
        /// Converts serialized UnityEngine.Color data back into a Color
        /// </summary>
        private static object Deserialize(StreamBuffer inStream, short length) {
            var color = new Color();
            var memColor = new byte[4 * 4];
            
            inStream.Read(memColor, 0, memColor.Length);
            var i = 0;

            Protocol.Deserialize(out color.r, memColor, ref i);
            Protocol.Deserialize(out color.g, memColor, ref i);
            Protocol.Deserialize(out color.b, memColor, ref i);
            Protocol.Deserialize(out color.a, memColor, ref i);

            return color;
        }

        /// <summary>
        /// Registers this custom type with Photon
        /// </summary>
        public static void Register() {
            PhotonPeer.RegisterType(typeof(Color), 1, Serialize, Deserialize);
        }
        
    }
}

