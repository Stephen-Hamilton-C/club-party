using UnityEngine;

namespace SHamilton.ClubParty.UI.Tabs {
    public class TabNavigationButton : ButtonBase {
        private enum Direction {
            Previous,
            Next,
        }

        [SerializeField] private Direction navigationDirection;
        [SerializeField] private TabGroup tabGroup;

        private void Update() {
            if (navigationDirection == Direction.Previous && Input.GetButtonDown("TabNavigationPrevious")) {
                OnClick();
            } else if (navigationDirection == Direction.Next && Input.GetButtonDown("TabNavigationNext")) {
                OnClick();
            }
        }

        protected override void OnClick() {
            if (navigationDirection == Direction.Previous) {
                tabGroup.PreviousTab();
            } else {
                tabGroup.NextTab();
            }
        }
    }
}
