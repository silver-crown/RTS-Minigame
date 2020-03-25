using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Progress.CameraSystem
{
    /// <summary>
    /// The camera mode for moving to a GameObject.
    /// </summary>
    public class CameraTransit : CameraMode
    {
        private Vector3 _startLocation;
        private GameObject _target;
        private float _progress;
        private float _transitTime;

        [SerializeField]
        private OnComplete _onComplete;

        [System.Serializable]
        public class OnComplete : UnityEvent <GameObject>
        {

        }

        public override void UpdateState()
        {
            if (_progress < _transitTime)
            {
                _progress += Time.deltaTime;
                _controller.transform.position = Vector3.Lerp(_startLocation, _target.transform.position, _progress / _transitTime);
            }
            else
            {
                _onComplete.Invoke(_target);
                _onComplete.RemoveAllListeners();

                if(_controller.IsInState(this))
                {
                    _controller.EnterDefaultState();
                }
            }
        }

        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {
            _target = null;
            _startLocation = new Vector3(0f, 0f, 0f);
            _progress = 0f;
            _transitTime = 0f;
            IsInitialised = false;
            _onComplete.RemoveAllListeners();
        }

        public override void Initialise()
        {
            throw new System.NotImplementedException();
        }

        public void Initialise(GameObject target, float transitPeriod, UnityAction<GameObject> onCompleteCallback = null)
        {
            _startLocation = _controller.transform.position;
            _progress = 0f;
            _target = target;
            _transitTime = transitPeriod;
            _onComplete.RemoveAllListeners();
            if (onCompleteCallback != null)
            {
                _onComplete.AddListener(onCompleteCallback);
            }
            IsInitialised = true;
        }

        protected override void OnAwake()
        {
            
        }
    }
}
