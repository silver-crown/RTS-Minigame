using HGC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS.DataModel;

namespace RTS
{
    public class SceneConfiguration : MonoBehaviour
    {
        [SerializeField]
        private ScenarioSettings _scenarioSetting;

        [SerializeField]
        private bool _hasLoadedScenes;

        public bool HasLoadedScenes { get { return _hasLoadedScenes; } }
        public ScenarioSettings ScenarioSettings { get { return _scenarioSetting; } }

        public void OnScenesLoaded()
        {
            if(_scenarioSetting == null)
                _scenarioSetting = ScriptableObjectFactory.Load<ScenarioSettings>("RTS/Settings/Scenario", true);

            Debug.Log("Have loaded scenario settings... now setting up the scenes: " + _scenarioSetting.Name);

            // ADD scene setup here
        }
    }
}
