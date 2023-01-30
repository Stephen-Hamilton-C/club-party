using UnityEngine;

namespace SHamilton.ClubParty.Ball {
    /// <summary>
    /// Draws a line from this transform to its parent
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class MouseTargetLine : MonoBehaviour {

        private LineRenderer _line;
        private Transform _parent;
    
        private void OnEnable() {
            _line = GetComponent<LineRenderer>();
            _parent = transform.parent;
            SetPositions();
        }

        private void LateUpdate() {
            SetPositions();
        }

        private void SetPositions() {
            _line.SetPositions(new[] {
                transform.position,
                _parent.position,
            });
        }
    }
}
