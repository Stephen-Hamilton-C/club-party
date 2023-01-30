using Photon.Pun;
using Photon.Realtime;
using SHamilton.ClubParty.Network;

namespace SHamilton.ClubParty.PowerUp {
    public abstract class OffensivePowerUpData : PowerUpData {
        public override void Selected() {
            // Show player select UI
            // (Disable players that already have this power up applied)
            throw new System.NotImplementedException();
        }

        public void ApplyToPlayer(Player player) {
            var character = player.GetCharacter();
            var view = character.GetComponent<PhotonView>();
            view.RPC("AddPowerUpComponentRPC", RpcTarget.AllBuffered, Name);

            RemoveFromStorage();
        }
    }
}