using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Progress.InputSystem
{
    public class HealthController : BarController
    {
        [System.Serializable]
        public class HealthUpdated : UnityEvent<Unit, float>
        {

        }

        [SerializeField]
        private int _minValue = 0;

        [SerializeField]
        private int _maxValue = 1;

        [SerializeField]
        private int _currentValue;

        [SerializeField]
        private HealthUpdated _onChanged;

        public void RegisterOnChanged(UnityAction<Unit, float> callback)
        {
            _onChanged.AddListener(callback);
        }

        public int MaxValue { get { return _maxValue; } }
        public int CurrentValue { get { return _currentValue; } }

        public override void Start()
        {
            base.Start();
        }

        public int Stat
        {
            get
            {
                return _currentValue;
            }

            set
            {
                if (value == _currentValue)
                    return;

                if (value > _maxValue)
                {
                    Debug.LogError("Cannot set value higher than max value: " + _maxValue, this);
                    return;
                }

                if (value < _minValue)
                {
                    Debug.LogError("Cannot set value lower than min value: " + _minValue, this);
                    return;
                }

                _currentValue = value;

                var percentage = GetValueAsPercentage();
                _onChanged.Invoke(_unit, percentage);

                // FIXME: hack response should be elsewhere in the gameplay code
                if (_currentValue <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        public float GetValueAsPercentage()
        {
            float percentage = (float)_currentValue / (float)_maxValue;

            float clampedValue = Mathf.Clamp(percentage, 0f, 1f);

            return clampedValue;
        }

        public override BarUI InstantiateUI(Transform parent, BarOverlayConfig config)
        {
            var healthbarUI = base.InstantiateUI(parent, config) as HealthBarUI;

            return healthbarUI;
        }
    }
}
