using System;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.InputSystem
{
    [System.Serializable]
    public class BarOverlayConfig
    {
        public string Name;
        public List<BarController> Controllers; 
        public List<BarUI> UIs;
        public System.Action<BarController> OnAddUI; 
        public System.Action<BarController> OnRemoveUI;
        public Vector2 Offset { get; private set; }
        public bool IsUIEnabled { get; private set; }

        public BarOverlayConfig(string barName, Vector2 offset, bool enabled = true) : this()
        {
            Name = barName;
            Offset = offset;
            IsUIEnabled = enabled;
        }

        public BarOverlayConfig()
        {
            Controllers = new List<BarController>();
            UIs = new List<BarUI>();
        }
    }

    public class SceneOverlayManager : MonoBehaviour
    {
        [SerializeField]
        private List<BarOverlayConfig> _configs;

        protected void Awake()
        {
            if(_configs.IsNullOrEmpty())
            {
                _configs.Add(new BarOverlayConfig("Health", Vector2.zero));
                _configs.Add(new BarOverlayConfig("Armour", new Vector2(0f, 20f)));
            }
        }

        public bool TryGetConfig(string configName, out BarOverlayConfig config, bool errorIfNull = false)
        {
            config = GetConfig(configName, errorIfNull);

            return config != null;
        }

        public BarOverlayConfig GetConfig(string configName, bool errorIfNull = false)
        {
            configName = configName.ToLower();

            var config = _configs.Find((c) => c.Name.ToLower() == configName);

            if (config == null)
            {
                if(errorIfNull)
                    Debug.LogError("Bar config does not exist for: " + configName);

                return null;
            }

            return config;
        }

        /// <summary>
        /// Creates a BarUI and assigns it to the passed in BarController object, then adds both to the Config.
        /// </summary>
        /// <param name="health"></param>
        public void Create(string barName, BarController controller)
        {
            Debug.Log("Creating " + barName + " for " + controller.name, this);

            BarOverlayConfig config;
            if (!TryGetConfig(barName, out config, true))
                return;

            Debug.Log("Found config for " + barName + " for " + controller.name, this);

            if (config.Controllers.Contains(controller))
            {
                Debug.LogError("This controller was already added for bar: " + barName);
                return;
            }

            Debug.Log("Instantiating " + barName + " for " + controller.name, this);
            var barUI = controller.InstantiateUI(transform, config);

            config.UIs.Add(barUI);
            config.Controllers.Add(controller);

            if (!config.IsUIEnabled)
                barUI.gameObject.SetActive(false);
        }

        private void RemoveBar(string uiName, BarController controller)
        {
            BarOverlayConfig config;
            if (!TryGetConfig(uiName, out config, true))
                return;

            config.OnRemoveUI.Invoke(controller);
        }

        private void AddBar(string uiName, BarController controller)
        {
            BarOverlayConfig config;
            if (!TryGetConfig(uiName, out config, true))
                return;

            config.OnAddUI.Invoke(controller);
        }
    }
}
