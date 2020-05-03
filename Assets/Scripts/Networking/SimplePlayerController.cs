using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

using Mirror;

/// <summary>
/// Simple player controler to test player networking.
/// </summary>
/// 
namespace RTS.Networking
{
    /// <summary>
    /// Used to select marine(s) and click where they will go to.
    /// This will work by sending mouse click position to the server.
    /// </summary>
    public class SimplePlayerController : NetworkBehaviour
    {
        /// <summary>
        /// The players camera this is where the RayCast will originate from when mouse is clicked.
        /// </summary>
        public Camera PlayerCamera;

        /// <summary>
        /// The tag of the marine unit that is to be selected
        /// </summary>
        public string PlayerUnitTag = "MarineUnit";

        /// <summary>
        /// Contains a list of all the selected marines.
        /// </summary>
        List<NetworkedMarine> SelectedUnits = new List<NetworkedMarine>();

        void Update()
        {
            // Left click to select marine
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.transform.tag);
                    if (hit.transform.tag == PlayerUnitTag)
                    {
                        if (hit.transform.GetComponent<NetworkedMarine>().netIdentity.hasAuthority)
                        {
                            SelectUnit(hit.transform.GetComponent<NetworkedMarine>(), Input.GetKey(KeyCode.LeftShift));
                        }
                        else
                        {
                            Debug.Log("Not clients marine ");
                        }
                    }
                    else if (hit.transform.CompareTag("Ground"))
                    {
                        DeSelectUnits();
                    }
                }
            }

            // Right click moves marine
            if (Input.GetMouseButtonDown(1) && (SelectedUnits.Count > 0))
            {
                Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("Ground"))
                    {
                        foreach (var marine in SelectedUnits)
                        {
                            marine.CmdMoveMarine(hit.point);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="multiSelectUnis"></param>
        public void SelectUnit(NetworkedMarine unit, bool multiSelectUnis)
        {
            if (!multiSelectUnis)
            {
                DeSelectUnits();
            }

            SelectedUnits.Add(unit);
            unit.ActiveHighLight();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeSelectUnits()
        {
            foreach (NetworkedMarine unit in SelectedUnits)
            {
                unit.DeActiveateHighLight();
            }
            SelectedUnits.Clear();
        }
    }

}