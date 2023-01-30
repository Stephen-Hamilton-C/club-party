using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI.Game {
    public class Arrow : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private float distanceFromTarget;
        [SerializeField] private Vector3 rotationOffset = new(90, 0, 0);

        private Logger _logger;
        private Transform _mouseTarget;
        private Transform _ball;
	
        private void Start() {
            _logger = new(this, debug);
            _mouseTarget = transform.parent;
            _ball = _mouseTarget.parent;
        }

        private void LateUpdate() {
            var mouseTargetPos = _mouseTarget.position;
            var ballPos = _ball.position;
            transform.position = Vector3.MoveTowards(mouseTargetPos, ballPos, distanceFromTarget);

            var targetToBall = ballPos - mouseTargetPos;
            if (targetToBall != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(targetToBall);
                transform.eulerAngles += rotationOffset;
            }
        }
    }
}
