using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI.Flair {
    // TODO: Need better name
    [RequireComponent(typeof(Selectable))]
    public class SelectableEffects : MonoBehaviour, 
        IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler, 
        ISelectHandler, IDeselectHandler {
        private Selectable _selectable;
        [SerializeField] private RectTransform childTransform;

        [SerializeField] private float idleTop = 25;
        [SerializeField] private float idleBottom = 25;
        
        [SerializeField] private float highlightedTop = 18; 
        [SerializeField] private float highlightedBottom = 32;
        
        [SerializeField] private float pressedTop = 32;
        [SerializeField] private float pressedBottom = 18;
        
        [SerializeField] private float disabledTop = 39;
        [SerializeField] private float disabledBottom = 11;

        private bool _previousInteractable;
        private bool _isPointerDown;
        private bool _isPointerInside;
        private bool _isSelected;

        private void Awake() {
            _selectable = GetComponent<Selectable>();
            _previousInteractable = _selectable.interactable;
            if (childTransform == null) {
                childTransform = GetComponentsInChildren<RectTransform>()[1];
            }
            
            UpdateState();
        }

        private void Update() {
            if (_previousInteractable != _selectable.interactable) {
                UpdateState();
                _previousInteractable = _selectable.interactable;
            }
        }

        private void UpdateState() {
            float top, bottom;
            if (!_selectable.IsInteractable()) {
                // Disabled
                top = disabledTop;
                bottom = disabledBottom;
            } else if (_isPointerDown) {
                // Pressed
                top = pressedTop;
                bottom = pressedBottom;
            } else if (_isSelected || _isPointerInside) {
                // Highlighted
                top = highlightedTop;
                bottom = highlightedBottom;
            } else {
                // Idle
                top = idleTop;
                bottom = idleBottom;
            }

            childTransform.SetTop(top);
            childTransform.SetBottom(bottom);
        }

        public void OnPointerDown(PointerEventData eventData) {
            _isPointerDown = true;
            UpdateState();
        }

        public void OnPointerUp(PointerEventData eventData) {
            _isPointerDown = false;
            UpdateState();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            _isPointerInside = true;
            UpdateState();
        }

        public void OnPointerExit(PointerEventData eventData) {
            _isPointerInside = false;
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
