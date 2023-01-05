using System;
using Photon.Realtime;
using UnityEngine;

namespace Network {
    public static class PhotonPlayerExtensions {
        /// <summary>
        /// Attempts to get the player's character. This cannot be called before the player's PlayerSetup.Awake() runs.
        /// </summary>
        /// <returns>The player's character</returns>
        /// <exception cref="InvalidOperationException">If the player's "Character" CustomProperty is null.
        /// This likely happens if this method is called before the player's PlayerSetup.Awake() runs.</exception>
        public static GameObject GetCharacter(this Player player) {
            var character = player.GetProperties().Character;
            if (character == null) {
                throw new InvalidOperationException(
                    "Attempted to get character before PlayerSetup.Awake() could be called for this player ("
                    + player + ")");
            }
            return character;
        }

        public static PlayerProperties GetProperties(this Player player) {
            return new PlayerProperties(player);
        }
    }
}