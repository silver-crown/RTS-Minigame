using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.CameraSystem
{
    /// <summary>
    /// Camera mode used for moving the camera according to input.
    /// </summary>
    public class CameraNormal : CameraMode
    {

        public override void UpdateState()
        {
            float forwardInput = _controller.player.GetAxis(Constants.RewiredInputConstants.cameraMoveForwardsOrBackwards);
            float sidewaysInput = _controller.player.GetAxis(Constants.RewiredInputConstants.cameraMoveSideways);

            if (forwardInput != 0f)
            {
                _controller.transform.Translate(Vector3.forward * Time.deltaTime * forwardInput * _controller.CameraSpeed);
            }

            if (sidewaysInput != 0f)
            {
                _controller.transform.Translate(Vector3.right * Time.deltaTime * sidewaysInput * _controller.CameraSpeed);
            }
        }

        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {
            
        }

        public override void Initialise()
        {
            IsInitialised = true;
        }

        protected override void OnAwake()
        {
            
        }
    }
}
