//by Neil Carter (NCarter)
//modified by Juan Castaneda (juanelo)
//
//added in-between GRP object to perform rotations on
//added auto-find main camera
//added un-initialized state, where script will do nothing
using UnityEngine;
using System.Collections;

namespace Progress.CameraSystem
{
    public class ParentedCameraFacingBillboard : MonoBehaviour
    {
        public Canvas m_Canvas;
        public Camera m_Camera;
        public bool amActive = false;
        public bool autoInit = false;
        public GameObject myContainer;

        public float myContainerYOffset = 1f;

        void Awake()
        {
            if (autoInit == true)
            {
                // note if camera is in a subscene it may not be loaded yet
                m_Camera = Camera.main;
                amActive = true;
            }

            if (m_Canvas == null)
                m_Canvas = GetComponent<Canvas>();

            myContainer = new GameObject();

            if (gameObject.transform.parent != null)
            {
                myContainer.transform.SetParent(gameObject.transform.parent, false);       
            }

            myContainer.transform.position = new Vector3(transform.position.x, transform.position.y + myContainerYOffset, transform.transform.position.z);
            myContainer.name = "Billboard_" + transform.gameObject.name;

            transform.SetParent(myContainer.transform, false);
        }

        //Orient the camera after all movement is completed this frame to avoid jittering
        void LateUpdate()
        {
            if (amActive == true)
            {
                if (m_Camera == null)
                {
                    m_Camera = Camera.main;
                    
                    if(m_Canvas != null && m_Canvas.worldCamera == null)
                    {
                        m_Canvas.worldCamera = m_Camera;
                    }
                }

                myContainer.transform.LookAt(myContainer.transform.position + m_Camera.transform.rotation * Vector3.back, m_Camera.transform.rotation * Vector3.up);
            }
        }
    }
}