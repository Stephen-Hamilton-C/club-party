using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.Tabs {
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler {

        [SerializeField] private TabGroup tabGroup;
        [SerializeField] public GameObject page;
        public Image background;

        public UnityEvent onTabSelected = new();
        public UnityEvent onTabDeselected = new();
        
        [SerializeField] private bool debug;

        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
            background = GetComponent<Image>();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            tabGroup.OnTabEnter(this);
        }

        public void OnPointerClick(PointerEventData eventData) {
            tabGroup.OnTabSelected(this);
        }

        public void OnPointerExit(PointerEventData eventData) {
            tabGroup.OnTabExit(this);
        }

        public void Select() {
            page.SetActive(true);
            onTabSelected.Invoke();
        }

        public void Deselect() {
            page.SetActive(false);
            onTabDeselected.Invoke();
        }
    }
}

