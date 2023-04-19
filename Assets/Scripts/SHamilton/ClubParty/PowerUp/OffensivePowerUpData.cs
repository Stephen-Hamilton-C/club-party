using Photon.Pun;
using Photon.Realtime;
using SHamilton.ClubParty.Network;
using SHamilton.ClubParty.UI.Flair;
using SHamilton.ClubParty.UI.PowerUp;

namespace SHamilton.ClubParty.PowerUp {
    public abstract class OffensivePowerUpData : PowerUpData {
        public override SelectableColor.Colors ButtonColor => SelectableColor.Colors.Red;

        public override void Selected() {
            // Show player select UI
            // (Disable players that already have this power up applied)
            OffensivePowerUpSelector.Instance.OffensivePowerUpSelected(this);
        }

        public void ApplyToPlayer(Player player) {
            var character = player.GetCharacter();
            var view = character.GetComponent<PhotonView>();
            view.RPC("AddPowerUpComponentRPC", view.Owner, Name);

            RemoveFromStorage();
        }
    }
}