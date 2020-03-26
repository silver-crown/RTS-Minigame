using HGC;
using Progress.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress
{
    public class SceneConfiguration : MonoBehaviour
    {
        [SerializeField]
        private ScenarioSettings _scenarioSetting;

        [SerializeField]
        private string _scenarioPath;

        [SerializeField]
        private bool _hasLoadedScenes;

        [SerializeField]
        private SceneOverlayManager _overlayManager;

        [SerializeField]
        private SymbolsOverlay _symbolOverlayManager;

        public bool HasLoadedScenes { get { return _hasLoadedScenes; } }
        public ScenarioSettings ScenarioSettings { get { return _scenarioSetting; } }

        public SceneOverlayManager GetOverlayManager()
        {
            return _overlayManager;
        }

        public SymbolsOverlay GetSymbolsOverlayManager()
        {
            return _symbolOverlayManager;
        }

        [System.Serializable]
        public struct Boundaries
        {
            public float zPositive;
            public float zNegative;
            public float xPositive;
            public float xNegative;
        }

        [SerializeField]
        private Boundaries bounds;

        public Boundaries GetBounds()
        {
            return bounds;
        }

        public void OnScenesLoaded()
        {
            _hasLoadedScenes = true;

            if(string.IsNullOrEmpty(_scenarioPath))
            {
                Debug.Log("No scenario settings to execute scenario setup", this);
                return;
            }

            if (_scenarioSetting == null)
                _scenarioSetting = ScriptableObjectFactory.Load<ScenarioSettings>(_scenarioPath, true);

            if(_scenarioSetting == null)
            {
                Debug.LogError("Can't load scenario settings from: " + _scenarioPath, this);
                return;
            }

            Debug.Log("Have loaded scenario settings... now setting up the scenes", _scenarioSetting);
            _scenarioSetting.ExecuteScenarioSetup();
        }
    }
}
