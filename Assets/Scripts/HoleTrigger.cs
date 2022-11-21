using System;
using UnityEngine;

public class HoleTrigger : MonoBehaviour {
    
    [SerializeField] private bool debug;

    private Logger _logger;
	
    private void Start() {
        _logger = new(this, debug);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            _logger.Log("Player ("+other.name+") made it into the hole!");
            GameManager.Instance.PlayerInHole(other.gameObject);
        }
    }
}
