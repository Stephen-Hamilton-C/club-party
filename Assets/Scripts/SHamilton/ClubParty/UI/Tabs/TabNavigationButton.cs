using UnityEngine;
using UnityEngine.InputSystem;

namespace SHamilton.ClubParty.UI.Tabs {
    public class TabNavigationButton : ButtonBase {
        private enum Direction {
            Previous,
            Next,
        }

        [SerializeField] private Direction navigationDirection;
        [SerializeField] private TabGroup tabGroup;

        private Controls _controls;
        private InputAction _tabNavigation;

        private void Awake() {
            _controls = new Controls();
            _tabNavigation = _controls.UI.TabNavigation;
        }

        private void OnEnable() {
            _tabNavigation.Enable();
        }

        private void OnDisable() {
            _tabNavigation.Disable();
        }

        private void Update() {
            if(!_tabNavigation.triggered) return;
            
            var value = _tabNavigation.ReadValue<float>();
            if (navigationDirection == Direction.Previous && value < 0) {
                OnClick();
            } else if (navigationDirection == Direction.Next && value > 0) {
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
