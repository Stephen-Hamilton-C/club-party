using SHamilton.ClubParty.Network;
using TMPro;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.MainMenu {
    /// <summary>
    /// Sets the PlayerName when the input field changes
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInput : MonoBehaviour {
    
        [SerializeField] private bool debug;

        private Logger _logger;
        private TMP_InputField _inputField;
	
        private void Start() {
            _logger = new(this, debug);
            _inputField = GetComponent<TMP_InputField>();
            _inputField.text = NetworkManager.PlayerName;
            _logger.Log("Text initialized to "+_inputField.text);
            
            _inputField.onEndEdit.AddListener(SetPlayerName);
        }

        private void SetPlayerName(string value) {
            _logger.Log("Player name changed to "+value);
            NetworkManager.PlayerName = value;
        }
    
    }
}
