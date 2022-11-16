using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour {
    private static GameManager instance;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("Cannot have multiple GameManager instances! This duplicate instance will be destroyed.");
            Destroy(this);
            return;
        }

        instance = this;
    }

    public void PlayerInHole(GameObject playerObject) {
        Rigidbody rb = playerObject.GetComponent<Rigidbody>();
        rb.position = new Vector3(0, playerObject.transform.localScale.y / 2, 0);
        rb.velocity = Vector3.zero;
    }
    
}
