using UnityEngine;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI.Flair {
    // TODO: Need better name
    [RequireComponent(typeof(Selectable))]
    public class SelectableEffects : SelectableState {
        [SerializeField] private RectTransform childTransform;

        [SerializeField] private float idleTop = 25;
        [SerializeField] private float idleBottom = 25;
        
        [SerializeField] private float highlightedTop = 18; 
        [SerializeField] private float highlightedBottom = 32;
        
        [SerializeField] private float pressedTop = 32;
        [SerializeField] private float pressedBottom = 18;
        
        [SerializeField] private float disabledTop = 39;
        [SerializeField] private float disabledBottom = 11;

        private void Start() {
            if (childTransform == null) {
                childTransform = GetComponentsInChildren<RectTransform>()[1];
            }
        }

        protected override void StateChanged(State state) {
            float top, bottom;
            switch (state) {
                case State.Disabled:
                    top = disabledTop;
                    bottom = disabledBottom;
                    break;
                case State.Pressed:
                    top = pressedTop;
                    bottom = pressedBottom;
                    break;
                case State.Highlighted:
                    top = highlightedTop;
                    bottom = highlightedBottom;
                    break;
                case State.Idle:
                default:
                    top = idleTop;
                    bottom = idleBottom;
                    break;
            }

            childTransform.SetTop(top);
            childTransform.SetBottom(bottom);
        }
    }
}
