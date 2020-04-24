﻿using System.Collections;
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
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit))
                {
                    // ActiveMarine.agent.SetDestination(hit.point);
                    Debug.Log(hit.transform.tag);
                    if (hit.transform.tag == PlayerUnitTag)
                    {
                        SelectUnit(hit.transform.GetComponent<NetworkedMarine>(), Input.GetKey(KeyCode.LeftShift));
                    }
                    else if(hit.transform.CompareTag("Ground"))
                    {
                        DeSelectUnits();
                    }
                }
            }
        }
        public void SelectUnit(NetworkedMarine unit, bool multiSelect)
        {
            if(!multiSelect)
            {
                DeSelectUnits();
            }

            SelectedUnits.Add(unit);
            unit.ActiveHighLight();
        }

        public void DeSelectUnits()
        {
            foreach(NetworkedMarine unit in SelectedUnits)
            {
                unit.DeActiveateHighLight();
            }
            SelectedUnits.Clear();
        }
    } 
}