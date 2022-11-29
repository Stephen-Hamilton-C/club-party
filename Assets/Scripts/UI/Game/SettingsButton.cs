using UnityEngine;

public class SettingsButton : MonoBehaviour {
    
    [SerializeField] private bool debug;

    private Logger _logger;
	
    private void Start() {
        _logger = new(this, debug);
    }
}
