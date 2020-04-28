using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

using Mirror;

/// <summary>
/// Simple player controller to test Networking with.
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
        public NetworkedMarine ActiveMarine;
        public Camera cam;
        public string PlayerUnitTag = "MarineUnit";

        List<NetworkedMarine> SelectedUnits = new List<NetworkedMarine>();


        void Update()
        {
            // Left click to select marine
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // ActiveMarine.agent.SetDestination(hit.point);
                    Debug.Log(hit.transform.tag);
                    if (hit.transform.tag == PlayerUnitTag)
                    {
                        if (hit.transform.GetComponent<NetworkedMarine>().netIdentity.hasAuthority)
                        {
                            SelectUnit(hit.transform.GetComponent<NetworkedMarine>(), Input.GetKey(KeyCode.LeftShift));
                        }
                        else
                        {
                            Debug.Log("Not your marine :(");
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
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
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
        public void SelectUnit(NetworkedMarine unit, bool multiSelect)
        {
            if (!multiSelect)
            {
                DeSelectUnits();
            }

            SelectedUnits.Add(unit);
            unit.ActiveHighLight();
        }

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