using UnityEngine;

namespace SHamilton.Util {
    public class SpinEffect : MonoBehaviour {
        [SerializeField] private bool debug;
        [SerializeField] private float xRotDegreesPerSecond;
        [SerializeField] private float yRotDegreesPerSecond;
        [SerializeField] private float zRotDegreesPerSecond;
        
        #if UNITY_EDITOR
        private float _lastXRotDPS;
        private float _lastYRotDPS;
        private float _lastZRotDPS;
        private Quaternion _origRot;
        #endif

        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
            #if UNITY_EDITOR
            _origRot = transform.rotation;
            RememberLast();
            #endif
        }

        private void Update() {
            #if UNITY_EDITOR
            if (_lastXRotDPS != xRotDegreesPerSecond || _lastYRotDPS != yRotDegreesPerSecond || 
                _lastZRotDPS != zRotDegreesPerSecond) {
                transform.rotation = _origRot;
            }
            #endif
            
            transform.Rotate(
                Time.deltaTime * xRotDegreesPerSecond,
                Time.deltaTime * yRotDegreesPerSecond,
                Time.deltaTime * zRotDegreesPerSecond
            );
            
            #if UNITY_EDITOR
            RememberLast();
            #endif
        }
        
        #if UNITY_EDITOR
        private void RememberLast() {
            _lastXRotDPS = xRotDegreesPerSecond;
            _lastYRotDPS = yRotDegreesPerSecond;
            _lastZRotDPS = zRotDegreesPerSecond;
        }
        #endif
    }
}
