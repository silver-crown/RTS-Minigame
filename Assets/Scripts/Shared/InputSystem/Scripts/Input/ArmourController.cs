using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Progress.InputSystem
{
    public class ArmourController : BarController
    {
        [System.Serializable]
        public class ArmourUpdated : UnityEvent<Unit, int>
        {

        }

        [SerializeField]
        private int _minValue = 0;
        [SerializeField]
        private int _maxValue = 4;
        [SerializeField]
        private int _currentValue;

        [SerializeField]
        private ArmourUpdated _onChanged;

        public void RegisterOnChanged(UnityAction<Unit, int> callback)
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

                if(value > _maxValue)
                {
                    Debug.LogError("Cannot set value higher than max value: " + _maxValue, this);
                    return;
                }

                if(value < _minValue)
                {
                    Debug.LogError("Cannot set value lower than min value: " + _minValue, this);
                    return;
                }

                _currentValue = value;

                _onChanged.Invoke(_unit, _currentValue);

                // FIXME: hack response should be elsewhere
                if (_currentValue <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        public override BarUI InstantiateUI(Transform parent, BarOverlayConfig config)
        {
            var armourbarUI = base.InstantiateUI(parent, config) as ArmourBarUI;

            return armourbarUI;
        }
    }
}
