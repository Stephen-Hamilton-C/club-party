using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI.Flair {
    [RequireComponent(typeof(Selectable))]
    public abstract class SelectableState : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler,
        ISelectHandler, IDeselectHandler
    {
        public enum State {
            Disabled,
            Highlighted,
            Pressed,
            Idle,
        }

        protected State SelectedState => _selectedState;
        
        private bool _previousInteractable;
        private bool _isPointerDown;
        private bool _isPointerInside;
        private bool _isSelected;
        private State _selectedState;
        private Selectable _selectable;

        protected virtual void Awake() {
            _selectable = GetComponent<Selectable>();
            _previousInteractable = _selectable.IsInteractable();
        }

        protected void OnEnable() {
            _isPointerDown = false;
            _isPointerInside = false;
            _isSelected = false;
            UpdateState();
        }

        protected virtual void Update() {
            var interactable = _selectable.IsInteractable();
            if (_previousInteractable != interactable) {
                UpdateState();
                _previousInteractable = interactable;
            }
        }

        protected virtual void StateChanged(State state) { }

        private void UpdateState() {
            var previousState = _selectedState;
            
            if (!_selectable.IsInteractable()) {
                _selectedState = State.Disabled;
            } else if (_isPointerDown) {
                _selectedState = State.Pressed;
            } else if (_isSelected || _isPointerInside) {
                _selectedState = State.Highlighted;
            } else {
                _selectedState = State.Idle;
            }

            if (previousState != _selectedState) {
                StateChanged(_selectedState);
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            _isPointerInside = true;
            UpdateState();
        }

        public void OnPointerExit(PointerEventData eventData) {
            _isPointerInside = false;
            UpdateState();
        }

        public void OnPointerDown(PointerEventData eventData) {
            _isPointerDown = true;
            UpdateState();
        }

        public void OnPointerUp(PointerEventData eventData) {
            _isPointerDown = false;
            UpdateState();
        }

        public void OnSelect(BaseEventData eventData) {
            _isSelected = true;
            UpdateState();
        }

        public void OnDeselect(BaseEventData eventData) {
            _isSelected = false;
            UpdateState();
        }
    }
}