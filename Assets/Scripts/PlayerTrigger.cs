using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour {

    [SerializeField] private bool debug;
    private Logger _logger;

    public delegate void PlayerTriggerEvent(GameObject playerObject);

    public event PlayerTriggerEvent OnPlayerEntered;
    public event PlayerTriggerEvent OnPlayerExited;
    
    void Awake() {
        _logger = new Logger(this);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            _logger.Log("A Player entered this trigger.");
            if (OnPlayerEntered != null)
                OnPlayerEntered(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            _logger.Log("A Player left this trigger.");
            if (OnPlayerExited != null)
                OnPlayerExited(other.gameObject);
        }
    }
}
