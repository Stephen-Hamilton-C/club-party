using TMPro;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.UI {
    [RequireComponent(typeof(TMP_Text))]
    public class TextBase : MonoBehaviour {
    
        [SerializeField] protected bool debug;

        protected TMP_Text Text;

        private void Start() {
            Text = GetComponent<TMP_Text>();
        }
    }
}

