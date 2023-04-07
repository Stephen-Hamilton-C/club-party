using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.Vote {
    [RequireComponent(typeof(Toggle))]
    [RequireComponent(typeof(Image))]
    public class VotingButton : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private Color selectedColor;
        [SerializeField] private float transitionTime = 0.25f;

        public Toggle toggle;

        public CourseData Course {
            get => _course;
            set {
                _course = value;
                _text.text = _course.courseName;
            }
        }
        private CourseData _course;
        
        private Logger _logger;
        private Image _image;
        private Color _deselectedColor;
        private TMP_Text _text;
	
        private void Start() {
            _logger = new(this, debug);
            toggle = GetComponent<Toggle>();
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<TMP_Text>();

            _deselectedColor = _image.color;
            toggle.onValueChanged.AddListener(ValueChanged);
        }

        private void ValueChanged(bool value) {
            var color = value ? selectedColor : _deselectedColor;
            _image.color = color;
            _logger.Log("Color set to "+color);
        }
    }
}

