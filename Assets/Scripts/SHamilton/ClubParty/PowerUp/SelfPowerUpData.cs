using Photon.Pun;
using SHamilton.ClubParty.Network;

namespace SHamilton.ClubParty.PowerUp {
    public abstract class SelfPowerUpData : PowerUpData {
        public override void Selected() {
            // Activate power up on self
            var view = NetworkManager.LocalCharacter.GetComponent<PhotonView>();
            view.RPC("AddPowerUpComponentRPC", RpcTarget.AllBuffered, Name);
            
            RemoveFromStorage();
        }
    }
}