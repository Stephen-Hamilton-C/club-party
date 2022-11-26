using Photon.Pun;
using UnityEngine;

/// <summary>
/// Listens for players who have touched this hole, then informs the GameManager
/// </summary>
public class HoleTrigger : MonoBehaviour {
    
    [SerializeField] private bool debug;

    private Logger _logger;
	
    private void Start() {
        _logger = new(this, debug);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            _logger.Log("Player ("+other.name+") made it into the hole!");

            var player = other.GetComponent<PhotonView>();
            GameManager.Instance.PlayerInHole(player.Owner);
        }
    }
}
