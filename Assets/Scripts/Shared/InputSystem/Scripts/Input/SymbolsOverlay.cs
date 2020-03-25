using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.InputSystem
{
    public class SymbolsOverlay : MonoBehaviour
    {
        [System.Serializable]
        public class SymbolOverlayConfig
        {
            public string Name;
            public List<SymbolsController> Controllers;
            public List<SymbolsUI> UIs;
            public System.Action<SymbolsController> OnAddUI;
            public System.Action<SymbolsController> OnRemoveUI;

            public SymbolOverlayConfig(string symbolName) : this()
            {
                Name = symbolName;
            }

            public SymbolOverlayConfig()
            {
                Controllers = new List<SymbolsController>();
                UIs = new List<SymbolsUI>();
            }
        }

        [SerializeField]
        private List<SymbolOverlayConfig> _configs;


        private void Awake()
        {
            if (_configs.IsNullOrEmpty())
            {
                _configs.Add(new SymbolOverlayConfig("Waypoints"));
                _configs.Add(new SymbolOverlayConfig("XP"));
            }
        }

        public void RemoveUI(string uiName, SymbolsController controller)
        {
            uiName = uiName.ToLower();

            var config = _configs.Find((c) => c.Name.ToLower() == uiName);

            if (config == null)
            {
                Debug.LogError("No config found for: " + uiName);
                return;
            }

            config.OnRemoveUI.Invoke(controller);
        }

        public void AddUI(string uiName, SymbolsController controller)
        {
            uiName = uiName.ToLower();

            var config = _configs.Find((c) => c.Name.ToLower() == uiName);

            if (config == null)
            {
                Debug.LogError("No config found for: " + uiName);
                return;
            }

            config.OnAddUI.Invoke(controller);
        }

        /// <summary>
        /// Creates a SymbolsUI and assigns it to the passed in SymbolsController object, then adds both to the Config.
        /// </summary>
        /// <param name="health"></param>
        public void AddToSymbolConfig(string symbolName, SymbolsController controller)
        {
            symbolName = symbolName.ToLower();

            var config = _configs.Find((c) => c.Name.ToLower() == symbolName);

            if (config == null)
            {
                Debug.LogError("Symbol config does not exist for: " + symbolName);
                return;
            }

            if (config.Controllers.Contains(controller))
            {
                Debug.LogError("This controller was already added for symbol: " + symbolName);
                return;
            }

            var symbolUI = controller.InstantiateUI(transform);

            config.UIs.Add(symbolUI);
            config.Controllers.Add(controller);
        }
    }
}
