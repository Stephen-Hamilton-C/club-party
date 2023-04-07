using System;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;

namespace SHamilton.ClubParty.Network {
    public static class PhotonPlayerExtensions {
        /// <summary>
        /// Attempts to get the player's character. This cannot be called before the player's PlayerSetup.Awake() runs.
        /// </summary>
        /// <returns>The player's character</returns>
        /// <exception cref="InvalidOperationException">If the player's "Character" CustomProperty is null.
        /// This likely happens if this method is called before the player's PlayerSetup.Awake() runs.</exception>
        public static GameObject GetCharacter(this Player player) {
            // var character = new PlayerProperties(player).Character;
            var character = (GameObject) player.CustomProperties[PropertyKeys.Character];
            if (character == null) {
                throw new InvalidOperationException(
                    "Attempted to get character before PlayerSetup.Awake() could be called for this player ("
                    + player + ")");
            }
            return character;
        }

        /// <summary>
        /// Sets the player's character. This should only be called once when the player's character is instantiated.
        /// </summary>
        /// <param name="character">The player's character</param>
        /// <exception cref="InvalidOperationException">If the character parameter given is null</exception>
        public static void SetCharacter(this Player player, GameObject character) {
            if (character == null) {
                throw new InvalidOperationException(
                    "Attempted to set character to null for this player ("
                                                    +player+")"
                );
            }

            player.CustomProperties[PropertyKeys.Character] = character;
        }

        public static int GetCurrentVote(this Player player) {
            var currentVote = (int)player.CustomProperties[PropertyKeys.CurrentVote];
            return currentVote;
        }

        public static void SetCurrentVote(this Player player, int vote) {
            player.CustomProperties[PropertyKeys.CurrentVote] = vote;
            var propChanges = new Hashtable() { { PropertyKeys.CurrentVote, vote } };
            player.SetCustomProperties(propChanges);
        }
    }
}