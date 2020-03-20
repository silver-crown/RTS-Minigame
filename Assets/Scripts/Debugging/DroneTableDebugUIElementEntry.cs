using RTS.Lua;
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
        public LuaObjectComponent Component { get; set; }

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
            if (Component == null)
            {
                Debug.LogError(GetType().Name +
                    ": Component was null. Set Component immedieately after instantiation.");
            }
        }

        private void Update()
        {
            if (!error)
            {
                _keyText.text = Key;
                _valueText.text = Component.Get(Key).CastToString();
            }
        }
    }
}