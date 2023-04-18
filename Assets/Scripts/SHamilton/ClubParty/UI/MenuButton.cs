using SHamilton.ClubParty.Ball;
using UnityEngine;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI {
    [RequireComponent(typeof(Button))]
    public class MenuButton : ButtonBase {
        [SerializeField] private GameObject menu;
        [SerializeField] private Button menuFocus;

        public void ToggleMenu() {
            menu.SetActive(!menu.activeSelf);
            if(menu.activeSelf)
                menuFocus.Select();
        }

        protected override void OnClick() {
            ToggleMenu();
        }
    }
}
