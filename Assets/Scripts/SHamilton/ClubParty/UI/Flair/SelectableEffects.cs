using UnityEngine;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI.Flair {
    // TODO: Make a Button version of this
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

        private State? _forcedState;

        protected override void Awake() {
            base.Awake();
            
            if (childTransform == null) {
                childTransform = GetComponentsInChildren<RectTransform>()[1];
            }
        }

        public void ForceState(State? state) {
            _forcedState = state;
            StateChanged(SelectedState);
        }

        public void ForceDisabledState() {
            ForceState(State.Disabled);
        }

        public void ForcePressedState() {
            ForceState(State.Pressed);
        }

        public void ForceHighlightedState() {
            ForceState(State.Highlighted);
        }

        public void ForceIdleState() {
            ForceState(State.Idle);
        }

        public void ClearForcedState() {
            ForceState(null);
        }

        protected override void StateChanged(State state) {
            if (_forcedState != null) {
                state = (State)_forcedState;
            }
            
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
