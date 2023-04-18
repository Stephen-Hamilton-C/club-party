using SHamilton.ClubParty.Ball;
using UnityEngine;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI {
    [RequireComponent(typeof(Button))]
    public class MenuButton : ButtonBase {
        [SerializeField] private GameObject menu;

        public void ToggleMenu() {
            menu.SetActive(!menu.activeSelf);
        }

        protected override void OnClick() {
            ToggleMenu();
        }
    }
}
