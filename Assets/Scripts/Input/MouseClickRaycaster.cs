using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class MouseClickRaycaster : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    MouseClickRaycastTarget target = hit.transform.GetComponent<MouseClickRaycastTarget>();

                    if (target != null)
                    {
                        target.Click();
                    }
                }
            }
        }
    }
}