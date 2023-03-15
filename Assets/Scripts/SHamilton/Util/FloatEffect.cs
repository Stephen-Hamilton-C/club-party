using UnityEngine;

namespace SHamilton.Util {
    public class FloatEffect : MonoBehaviour {

        public enum Axis {
            X, Y, Z
        }
    
        [SerializeField] private bool debug;
        [SerializeField] private float floatFrequency = 1;
        [SerializeField] private float floatAmplitude = 1;
        [SerializeField] private Axis floatAxis = Axis.Y;
        #if UNITY_EDITOR
        private float _lastFloatFrequency;
        private float _lastFloatAmplitude;
        private Axis _lastFloatAxis;
        private Vector3 _origPosition;
        #endif

        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
            
            #if UNITY_EDITOR
            _origPosition = transform.position;
            RememberLast();
            #endif
        }

        private void Update() {
            #if UNITY_EDITOR
            if (_lastFloatAmplitude != floatAmplitude || _lastFloatFrequency != floatFrequency ||
                _lastFloatAxis != floatAxis)
            {
                transform.position = _origPosition;
            }
            #endif
            
            var pos = transform.position;
            var floatIncrement = Mathf.Sin (Time.fixedTime * Mathf.PI * floatFrequency) * floatAmplitude;
            switch (floatAxis) {
                case Axis.X:
                    pos.x += floatIncrement;
                    break;
                case Axis.Y:
                    pos.y += floatIncrement;
                    break;
                case Axis.Z:
                    pos.z += floatIncrement;
                    break;
            }
            transform.position = pos;
            
            #if UNITY_EDITOR
            RememberLast();
            #endif
        }
        
        #if UNITY_EDITOR
        private void RememberLast() {
            _lastFloatFrequency = floatFrequency;
            _lastFloatAmplitude = floatAmplitude;
            _lastFloatAxis = floatAxis;
        }
        #endif
    }
}
