using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.InputSystem
{
    public abstract class BarController : MonoBehaviour
    {
        [SerializeField]
        protected Unit _unit;

        [SerializeField]
        protected SceneConfiguration _sceneConfig;

        [SerializeField]
        protected SceneOverlayManager _overheadUI;

        [SerializeField]
        private BarUI _barUI;

        [SerializeField]
        private Collider _unitCollider;

        [Header("Setup UI Configuration")]
        [SerializeField]
        public GameObject BarPrefab;

        [SerializeField]
        private GameObject _barPrefabInstance;

        [SerializeField]
        private string _configName;

        public string ConfigName
        {
            get
            {
                return _configName;
            }
        }

        public Unit Unit
        {
            get { return _unit; }
        }
        public SceneConfiguration SceneConfig
        {
            get { return _sceneConfig; }
        }
        public SceneOverlayManager OverheadUI
        {
            get { return _overheadUI; }
        }
        public BarUI BarUI
        {
            get { return _barUI; }
        }
        public Collider UnitCollider
        {
            get { return _unitCollider; }
        }

        public virtual void Awake()
        {
            if (_unit == null)
                _unit = GetComponentInParent<Unit>();

            if (_unitCollider == null)
                _unitCollider = _unit.GetComponent<Collider>();

            if (_sceneConfig == null)
                _sceneConfig = GetComponentInParent<SceneConfiguration>();

            if (_overheadUI == null)
                _overheadUI = _sceneConfig.GetComponentInChildren<SceneOverlayManager>();

        }

        public virtual void Start()
        {
            if (_configName != null)
            {
                _overheadUI.Create(_configName, this);
            }
        }

        public BarUI GetUI()
        {
            return _barUI;
        }

        public virtual BarUI InstantiateUI(Transform parent, BarOverlayConfig config)
        {
            if (_barUI != null)
            {
                return _barUI;
            }

            _barPrefabInstance = Instantiate(BarPrefab, parent);
            var rect = _barPrefabInstance.gameObject.GetComponent<RectTransform>();
            var newOffset = new Vector2(rect.offsetMin.x, rect.offsetMin.y + config.Offset.y);
            rect.offsetMin = newOffset;

            _barUI = _barPrefabInstance.GetComponent<BarUI>();
            _barUI.SetController(this);

            return _barUI;
        }

        public Bounds GetBounds ()
        {
            return _unitCollider.bounds;
        }
    }
}
