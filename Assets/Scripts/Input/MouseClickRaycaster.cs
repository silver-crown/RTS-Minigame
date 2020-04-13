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
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
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