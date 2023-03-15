using Photon.Pun;
using Photon.Realtime;
using SHamilton.ClubParty.Network;
using SHamilton.ClubParty.UI.PowerUp;
using UnityEngine;

namespace SHamilton.ClubParty.PowerUp {
    public abstract class OffensivePowerUpData : PowerUpData {
        public override Color BackgroundColor => Color.red;
        public override Color ForegroundColor => Color.white;

        public override void Selected() {
            // Show player select UI
            // (Disable players that already have this power up applied)
            OffensivePowerUpSelector.Instance.OffensivePowerUpSelected(this);
        }

        public void ApplyToPlayer(Player player) {
            var character = player.GetCharacter();
            var view = character.GetComponent<PhotonView>();
            view.RPC("AddPowerUpComponentRPC", RpcTarget.AllBuffered, Name);

            RemoveFromStorage();
        }
    }
}