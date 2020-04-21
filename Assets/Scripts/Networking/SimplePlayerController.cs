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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // if left click
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit))
                {
                    ActiveMarine.agent.SetDestination(hit.point);
                }
            }
        }
    }
}