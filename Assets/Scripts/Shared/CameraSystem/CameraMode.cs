using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.CameraSystem
{
    public abstract class CameraMode : MonoBehaviour
    {
        [SerializeField]
        protected CameraController _controller;
        public bool IsInitialised { get; protected set; }

        private void Awake()
        {
            // do code common to all classes
            _controller.RegisterState(this);
            OnAwake();
        }

        protected abstract void OnAwake();

        //  This should have the API that the CameraController uses to transition in and out of Camera Modes.

        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void UpdateState();
        public abstract void Initialise();
    }
}
