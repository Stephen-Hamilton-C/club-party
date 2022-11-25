using ExitGames.Client.Photon;
using UnityEngine;

namespace Serializer {
    public static class ColorSerializer {

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

        public static void Register() {
            PhotonPeer.RegisterType(typeof(Color), 1, Serialize, Deserialize);
        }
        
    }
}

