using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public enum CIStrategy
    {
        ResourceExpansion,
        HuntAndDestroy
    }

    public class ExampleEnemyScript : MonoBehaviour
    {
        [SerializeField]
        private SceneConfiguration _sceneConfig;

        private CIStrategy _strategy;

        void Awake()
        {
            if (_sceneConfig == null)
                _sceneConfig = GetComponentInParent<SceneConfiguration>();

            _strategy = _sceneConfig.ScenarioSettings.Strategy;
        }

        void Reset()
        {
            if (_sceneConfig == null)
                _sceneConfig = GetComponentInParent<SceneConfiguration>();
        }
    }
}
