using UnityEngine;

public class HoleCollider : MonoBehaviour {
    
    public bool debug;

    public delegate void CharacterEvent(GameObject character);
    public event CharacterEvent OnCharacterCollided;

    private Logger _logger;
	
    private void Start() {
        _logger = new(this, debug);
    }

    private void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.CompareTag("Player")) return;
        OnCharacterCollided?.Invoke(collision.gameObject);
    }
}
