using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI.Flair {
    [RequireComponent(typeof(Selectable))]
    [ExecuteInEditMode]
    public class SelectableColor : MonoBehaviour {
        
        public delegate void SelectableSpritesEvent(SelectableSprites sprites);
        public event SelectableSpritesEvent OnSpritesChanged;

        public enum Colors {
            Red,
            Orange,
            Blue,
            Green,
            Black,
            White,
        }

        public Colors Color {
            get => color;
            set {
                color = value;
                UpdateSprites();
            }
        }
        [SerializeField] private Colors color = Colors.White;

        private Dictionary<Colors, SelectableSprites> _sprites;

        private SelectableSprites _currentSprites;
        private Selectable _selectable;
        private TMP_Text[] _buttonTexts = {};
        
        #if UNITY_EDITOR
        private Colors _lastColor;
        #endif

        private void InitializeSprites() {
            if (_selectable is Slider) {
                _sprites = new Dictionary<Colors, SelectableSprites> {
                    { Colors.Red, Resources.Load<SelectableSprites>("UI/SliderSprites/Red") },
                    { Colors.Orange, Resources.Load<SelectableSprites>("UI/SliderSprites/Orange") },
                    { Colors.Blue, Resources.Load<SelectableSprites>("UI/SliderSprites/Blue") },
                    { Colors.Green, Resources.Load<SelectableSprites>("UI/SliderSprites/Green") },
                    { Colors.Black, Resources.Load<SelectableSprites>("UI/SliderSprites/Black") },
                    { Colors.White, Resources.Load<SelectableSprites>("UI/SliderSprites/White") },
                };
            } else {
                _sprites = new Dictionary<Colors, SelectableSprites> {
                    { Colors.Red, Resources.Load<SelectableSprites>("UI/ButtonSprites/Red") },
                    { Colors.Orange, Resources.Load<SelectableSprites>("UI/ButtonSprites/Orange") },
                    { Colors.Blue, Resources.Load<SelectableSprites>("UI/ButtonSprites/Blue") },
                    { Colors.Green, Resources.Load<SelectableSprites>("UI/ButtonSprites/Green") },
                    { Colors.Black, Resources.Load<SelectableSprites>("UI/ButtonSprites/Black") },
                    { Colors.White, Resources.Load<SelectableSprites>("UI/ButtonSprites/White") },
                };
            }
        }

        private void Start() {
            _selectable = GetComponent<Selectable>();
            _buttonTexts = GetComponentsInChildren<TMP_Text>();
            
            UpdateSprites();
        }

        #if UNITY_EDITOR
        private void Update() {
            if (Application.isPlaying) return;
            
            if(_lastColor != color)
                UpdateSprites();
            _lastColor = color;
        }
        #endif

        private void UpdateSprites() {
            if(_sprites == null) InitializeSprites();
            _currentSprites = _sprites![color];

            var spriteState = new SpriteState {
                disabledSprite = _currentSprites.disabled,
                highlightedSprite = _currentSprites.highlighted,
                pressedSprite = _currentSprites.pressed,
                selectedSprite = _currentSprites.highlighted,
            };

            _selectable.spriteState = spriteState;
            _selectable.image.sprite = _currentSprites.idle;

            foreach(var buttonText in _buttonTexts) {
                var textColor = _currentSprites.textColor;
                textColor.a = buttonText.color.a;
                buttonText.color = textColor;
            }

            OnSpritesChanged?.Invoke(_currentSprites);
        }
    }
}
