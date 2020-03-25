using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace Progress.CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private CameraMode _currentCameraMode;
        [SerializeField]
        private List<CameraMode> _allCameraModes = new List<CameraMode>();

        private int playerId;
        public Player player { get; private set; }
        [SerializeField]
        public float CameraSpeed { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            playerId = 0;
            player = ReInput.players.GetPlayer(playerId);
            CameraSpeed = 3f;

            CameraMode newMode = _allCameraModes.Find(x => x.GetType() == typeof(CameraNormal));

            if (newMode != null)
            {
                if (!newMode.IsInitialised)
                    newMode.Initialise();
                newMode.EnterState();
                _currentCameraMode = newMode;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (player.GetAxis(Constants.RewiredInputConstants.cameraMoveForwardsOrBackwards) != 0f || player.GetAxis(Constants.RewiredInputConstants.cameraMoveSideways) != 0f)
            {
                if (_currentCameraMode.GetType() != typeof(CameraNormal))
                {
                    CameraMode newMode = _allCameraModes.Find(x => x.GetType() == typeof(CameraNormal));

                    if (newMode != null)
                    {
                        if (newMode.IsInitialised)
                        {
                            _currentCameraMode.ExitState();
                            newMode.EnterState();
                            _currentCameraMode = newMode;
                        }
                        else
                        {
                            Debug.LogWarning("That camera mode is not yet initialised, do that first");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Can't find that camera mode in the list");
                    }
                }
            }

            ////  This was just to test that the CameraTransit mode works, it's not the actual logic for entering it
            //if (player.GetButtonDown(Constants.InputConstants.mouse2))
            //{
            //    if (_currentCameraMode.GetType() != typeof(CameraTransit))
            //    {
            //        CameraTransit newMode = (CameraTransit)_allCameraModes.Find(x => x.GetType() == typeof(CameraTransit));
            //        Vector3 travel = new Vector3(5f, 0f, 5f);
            //        newMode.Initialise(transform.position + travel, 3f);

            //        if (newMode != null && newMode.IsInitialised)
            //        {
            //            _currentCameraMode.ExitState();
            //            newMode.EnterState();
            //            _currentCameraMode = newMode;
            //        }
            //    }
            //}
        }

        private void LateUpdate()
        {
            _currentCameraMode.UpdateState();
        }

        public void RegisterState (CameraMode state)
        {
            if (!_allCameraModes.Contains(state))
            {
                _allCameraModes.Add(state);
            }
        }

        /// <summary>
        /// Returns a CameraMode of the passed in type, if one exists. Null otherwise.
        /// </summary>
        /// <typeparam name="TCameraMode"></typeparam>
        /// <returns></returns>
        public TCameraMode GetCameraMode <TCameraMode> () where TCameraMode : CameraMode
        {
            CameraMode mode = _allCameraModes.Find(x => x.GetType() == typeof(TCameraMode));

            if (mode == null)
            {
                Debug.LogWarning("Can't find that camera mode in the list");
                return null;
            }

            return (TCameraMode)mode;
        }

        public void SetCameraMode <TCameraMode> () where TCameraMode : CameraMode
        {
            if (_currentCameraMode.GetType() != typeof(TCameraMode))
            {
                CameraMode newMode = _allCameraModes.Find(x => x.GetType() == typeof(TCameraMode));

                if (newMode != null)
                {
                    if (newMode.IsInitialised)
                    {
                        _currentCameraMode.ExitState();
                        newMode.EnterState();
                        _currentCameraMode = newMode;
                    }
                    else
                    {
                        Debug.LogWarning("That camera mode is not yet initialised, do that first");
                    }
                }
                else
                {
                    Debug.LogWarning("Can't find that camera mode in the list");
                }
            }
            else
            {
                Debug.LogWarning("Desired camera mode is already the current camera mode");
            }
        }

        public void EnterDefaultState ()
        {
            SetCameraMode<CameraNormal>();
        }

        public bool IsInDefaultState()
        {
            return _currentCameraMode.GetType() == typeof(CameraNormal);
        }

        public bool IsInState<TCameraMode>()
            where TCameraMode : CameraMode
        {
            return _currentCameraMode.GetType() == typeof(TCameraMode);
        }

        public bool IsInState(CameraMode mode)
        {
            return _currentCameraMode.GetType() == mode.GetType();
        }
    }
}
