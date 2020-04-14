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
            //public List<SymbolsController> Controllers;
            public GameObject prefab;
            public List<SymbolsUI> UIs;
            public System.Action<SymbolsContainer> OnAddUI;
            public System.Action<SymbolsContainer> OnRemoveUI;

            public SymbolOverlayConfig(string symbolName) : this()
            {
                Name = symbolName;
            }

            public SymbolOverlayConfig()
            {
                //Controllers = new List<SymbolsController>();
                UIs = new List<SymbolsUI>();
            }

            public GameObject InstantiateUI (Transform parent)
            {
                if (prefab == null)
                {
                    Debug.LogError("Config " + Name + " has no prefab");
                    return null;
                }
                GameObject instance = Instantiate(prefab, parent);
                SymbolsUI ui = instance.GetComponent<SymbolsUI>();
                if (ui == null)
                {
                    Debug.LogError("Prefab was missing SymbolsUI component when config instantiated it");
                    return null;
                }
                UIs.Add(ui);
                return instance;
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

        public void RemoveUI(string uiName, SymbolsContainer container)
        {
            uiName = uiName.ToLower();

            var config = _configs.Find((c) => c.Name.ToLower() == uiName);

            if (config == null)
            {
                Debug.LogError("No config found for: " + uiName);
                return;
            }

            config.OnRemoveUI.Invoke(container);
        }

        public void AddUI(string uiName, SymbolsContainer container)
        {
            uiName = uiName.ToLower();

            var config = _configs.Find((c) => c.Name.ToLower() == uiName);

            if (config == null)
            {
                Debug.LogError("No config found for: " + uiName);
                return;
            }

            config.OnAddUI.Invoke(container);
        }

        /// <summary>
        /// Creates a SymbolsUI and assigns it to the passed in SymbolsController object, then adds both to the Config.
        /// </summary>
        /// <param name="health"></param>
        public GameObject AddToSymbolConfig(string symbolName)
        {
            symbolName = symbolName.ToLower();

            var config = _configs.Find((c) => c.Name.ToLower() == symbolName);

            if (config == null)
            {
                Debug.LogError("Symbol config does not exist for: " + symbolName);
                return null;
            }

            //if (config.Controllers.Contains(controller))
            //{
            //    Debug.LogError("This controller was already added for symbol: " + symbolName);
            //    return;
            //}

            var symbolUI = config.InstantiateUI(transform);

            return symbolUI;

            //config.UIs.Add(symbolUI);
            //config.Controllers.Add(controller);
        }
    }
}
