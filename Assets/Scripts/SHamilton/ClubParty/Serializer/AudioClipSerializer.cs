using System;
using System.Collections.Generic;
using System.Text;
using ExitGames.Client.Photon;
using UnityEngine;

namespace SHamilton.ClubParty.Serializer {
    
    /// <summary>
    /// AudioClips are a lot to serialize, so this is handled a little differently.
    /// Rather than serializing all the data in an AudioClip,
    /// it should instead be placed in the Resources/Sounds folder.
    /// This then serializes the name of the AudioClip, and when deserializing, it grabs the AudioClip from Resources.
    /// This also caches the name of the AudioClip with the AudioClip itself, so each clip is loaded only once.
    /// </summary>
    public static class AudioClipSerializer {

        /// <summary>
        /// All of the previously played AudioClips. This prevents constant reading from Resources
        /// </summary>
        private static readonly Dictionary<string, AudioClip> Cache = new();

        private static short Serialize(StreamBuffer outStream, object customObject) {
            // Convert the AudioClip into a string
            var clip = (AudioClip)customObject;
            var clipNameBytes = Encoding.UTF8.GetBytes(clip.name);
            
            outStream.Write(clipNameBytes, 0, clipNameBytes.Length);

            return (short)clipNameBytes.Length;
        }

        private static object Deserialize(StreamBuffer inStream, short length) {
            // Convert the byte array into a string
            var clipNameBytes = new byte[length];
            inStream.Read(clipNameBytes, 0, length);
            var clipName = Encoding.UTF8.GetString(clipNameBytes, 0, length);

            // Check cache for an already existing clip
            if (Cache.TryGetValue(clipName, out var clip)) {
                return clip;
            }
            
            // Not in cache, try to load AudioClip from Resources
            clip = Resources.Load<AudioClip>("Sounds/" + clipName);
            if (clip == null) {
                // AudioClip not found in Resources/Sounds
                throw new InvalidOperationException(
                    "Tried to deserialize an AudioClip that was not found in Resources/Sounds! " +
                    "Make sure the AudioClip \"" + clipName + "\" is in Resources/Sounds."
                );
            }

            // Add to cache and return the deserialized clip
            Cache.Add(clip.name, clip);
            return clip;
        }

        /// <summary>
        /// Registers this custom type with Photon
        /// </summary>
        public static void Register() {
            PhotonPeer.RegisterType(typeof(AudioClip), 2, Serialize, Deserialize);
        }
        
    }
}

