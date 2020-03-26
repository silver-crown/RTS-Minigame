using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.CameraSystem
{
    /// <summary>
    /// Camera mode for following a target.
    /// </summary>
    public class CameraFollow : CameraMode
    {
        private GameObject _followTarget;
        private Vector3 _offset;

        public override void UpdateState()
        {
            _controller.transform.position = _followTarget.transform.position + _offset;
        }

        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {
            _followTarget = null;
            IsInitialised = false;
        }

        public override void Initialise()
        {
            throw new System.NotImplementedException();
        }

        public void Initialise (GameObject target, Vector3 offset = default)
        {
            _followTarget = target;
            _offset = offset;
            IsInitialised = true;
        }

        protected override void OnAwake()
        {
            
        }
    }
}
