using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MouseTargetLine : MonoBehaviour {

    private LineRenderer _line;
    private Transform _ball;
    
    private void Start() {
        _line = GetComponent<LineRenderer>();
        _ball = transform.parent;
    }

    private void LateUpdate() {
        _line.SetPositions(new[] {
            transform.position,
            _ball.position,
        });
    }
}
