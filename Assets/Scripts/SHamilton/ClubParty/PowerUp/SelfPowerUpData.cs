using Photon.Pun;
using SHamilton.ClubParty.Network;
using UnityEngine;

namespace SHamilton.ClubParty.PowerUp {
    public abstract class SelfPowerUpData : PowerUpData {
        public override Color BackgroundColor => Color.green;
        public override Color ForegroundColor => Color.black;
        public override bool CanSelect =>
            base.CanSelect && !NetworkManager.LocalCharacter.TryGetComponent(ComponentType, out _);

        public override void Selected() {
            var view = NetworkManager.LocalCharacter.GetComponent<PhotonView>();
            
            // Activate power up on self
            view.RPC("AddPowerUpComponentRPC", RpcTarget.AllBuffered, Name);
            
            RemoveFromStorage();
        }
    }
}