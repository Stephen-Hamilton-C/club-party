using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.Tabs {
    public class TabGroup : MonoBehaviour {

        [SerializeField] private bool debug;
        [SerializeField] private Sprite tabIdle;
        [SerializeField] private Sprite tabHover;
        [SerializeField] private Sprite tabActive;
        [SerializeField, CanBeNull] private TabButton selectedTab;
        
        private Logger _logger;
        private TabButton[] _tabButtons;
	
        private void Start() {
            _logger = new(this, debug);
            _tabButtons = GetComponentsInChildren<TabButton>();
            if(selectedTab)
                OnTabSelected(selectedTab);
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

            button.Select();
        }

        public void PreviousTab() {
            var previousIndex = Array.IndexOf(_tabButtons, selectedTab) - 1;
            if (previousIndex < 0)
                previousIndex = _tabButtons.Length - 1;
            
            _logger.Log("Going to tab at previous index: "+previousIndex);
            OnTabSelected(_tabButtons[previousIndex]);
        }

        public void NextTab() {
            var nextIndex = (Array.IndexOf(_tabButtons, selectedTab) + 1) % _tabButtons.Length;
            _logger.Log("Going to tab at next index: "+nextIndex);
            OnTabSelected(_tabButtons[nextIndex]);
        }

        private void ResetTabs() {
            foreach (var button in _tabButtons.Where(button => button != selectedTab)) {
                button.background.sprite = tabIdle;
            }
        }
    }
}

