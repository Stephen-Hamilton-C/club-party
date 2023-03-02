using System;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.Editor {
    [ExecuteInEditMode]
    public class PickupYPositioner : MonoBehaviour {
    
        [SerializeField] private bool debug;
        [SerializeField] private LayerMask raycastMask;

        private Logger _logger;
	
        private void Start() {
            _logger = new(this, debug);
        }

        private void LateUpdate() {
            if (!Application.isEditor) return;
            if (Physics.Raycast(transform.position, -transform.up, out var hit, 1000f, raycastMask.value)) {
                var pos = transform.position;
                pos.y = hit.point.y + transform.localScale.y;
                transform.position = pos;
            }
        }
    }
}

