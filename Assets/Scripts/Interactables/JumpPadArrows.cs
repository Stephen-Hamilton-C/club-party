using UnityEngine;

namespace Interactables {
    public class JumpPadArrows : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private float rotDegreesPerSecond = 1;
        [SerializeField] private float floatFrequency = 1;
        [SerializeField] private float floatAmplitude = 1;
        #if UNITY_EDITOR
        private float _lastRotDegreesPerSecond;
        private float _lastFloatFrequency;
        private float _lastFloatAmplitude;
        private Vector3 _origPosition;
        private Quaternion _origRot;
        #endif

        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
            #if UNITY_EDITOR
            _origPosition = transform.position;
            _origRot = transform.rotation;
            RememberLast();
            #endif
        }

        private void Update() {
            #if UNITY_EDITOR
            if (_lastRotDegreesPerSecond != rotDegreesPerSecond || _lastFloatAmplitude != floatAmplitude ||
                _lastFloatFrequency != floatFrequency) {
                transform.position = _origPosition;
                transform.rotation = _origRot;
            }
            #endif
            
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotDegreesPerSecond));
            var pos = transform.position;
            pos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * floatFrequency) * floatAmplitude;
            transform.position = pos;
            
            #if UNITY_EDITOR
            RememberLast();
            #endif
        }
        
        #if UNITY_EDITOR
        private void RememberLast() {
            _lastRotDegreesPerSecond = rotDegreesPerSecond;
            _lastFloatFrequency = floatFrequency;
            _lastFloatAmplitude = floatAmplitude;
        }
        #endif
    }
}
