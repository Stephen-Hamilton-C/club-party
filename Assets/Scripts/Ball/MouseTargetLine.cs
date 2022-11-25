using UnityEngine;

namespace Ball {
    [RequireComponent(typeof(LineRenderer))]
    public class MouseTargetLine : MonoBehaviour {

        private LineRenderer _line;
        private Transform _ball;
    
        private void OnEnable() {
            _line = GetComponent<LineRenderer>();
            _ball = transform.parent;
            SetPositions();
        }

        private void LateUpdate() {
            SetPositions();
        }

        private void SetPositions() {
            _line.SetPositions(new[] {
                transform.position,
                _ball.position,
            });
        }
    }
}
