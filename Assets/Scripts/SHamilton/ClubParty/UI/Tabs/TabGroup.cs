using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.Tabs {
    public class TabGroup : MonoBehaviour {

        [SerializeField] private bool debug;
        [SerializeField] private List<GameObject> pages = new();
        [SerializeField] private Sprite tabIdle;
        [SerializeField] private Sprite tabHover;
        [SerializeField] private Sprite tabActive;
        [SerializeField, CanBeNull] private TabButton selectedTab;
        
        private Logger _logger;
        private List<TabButton> _tabButtons = new();
	
        private void Awake() {
            _logger = new(this, debug);
        }

        public void Subscribe(TabButton button) {
            _tabButtons.Add(button);
            if (button == selectedTab) {
                _logger.Log("Found default tab.");
                OnTabSelected(button);
            }
        }

        public void OnTabEnter(TabButton button) {
            _logger.Log("OnTabEnter: "+button);
            ResetTabs();
            if (button != selectedTab) {
                _logger.Log("Tab is not selected: "+button);
                button.background.sprite = tabHover;
            }
        }

        public void OnTabExit(TabButton button) {
            _logger.Log("OnTabExit: " + button);
            ResetTabs();
        }

        public void OnTabSelected(TabButton button) {
            _logger.Log("OnTabSelected: "+button);
            if (selectedTab != null) {
                _logger.Log("Deselect current tab: "+selectedTab);
                selectedTab.Deselect();
            }

            selectedTab = button;
            
            ResetTabs();
            button.background.sprite = tabActive;

            var tabIndex = button.transform.GetSiblingIndex();
            for (int i = 0; i < pages.Count; i++) {
                pages[i].SetActive(i == tabIndex);
            }
            
            button.Select();
        }

        private void ResetTabs() {
            foreach (var button in _tabButtons.Where(button => button != selectedTab)) {
                button.background.sprite = tabIdle;
            }
        }
    }
}

