using UnityEngine;
using UnityEngine.UI;

namespace RTS.UI.Debugging
{
    public class DroneTableDebugUIElementEntry : MonoBehaviour
    {
        [SerializeField] private Text _keyText;
        [SerializeField] private Text _valueText;

        private bool error = false;

        public string Key { get; set; }
        public Actor Actor { get; set; }

        private void Start()
        {
            if (_keyText == null)
            {
                Debug.LogError(GetType().Name + ": _keyText was null. Set Key Text in the inspector.");
                error = true;
            }
            if (_valueText == null)
            {
                Debug.LogError(GetType().Name + ": _valueText was null. Set Value Text in the inspector.");
                error = true;
            }
            if (Actor == null)
            {
                Debug.LogError(GetType().Name + ": Actor was null. Set Actor immedieately after instantiation.");
            }
        }

        private void Update()
        {
            if (!error)
            {
                _keyText.text = Key;
                _valueText.text = Actor.GetValue(Key).CastToString();
            }
        }
    }
}