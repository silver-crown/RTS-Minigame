using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

using UnityEngine.AI;


namespace RTS.Networking
{
    /// <summary>
    /// Version of the actor class to work with networking
    /// </summary>
    public class NetworkedActor : NetworkBehaviour
    {
        #region Movement and Pathfinding

        bool Walking;

        [Header("Movement")]
        public GameObject[] Waypoints;
        public GameObject TargetDestination;

        public NavMeshAgent agent;

        #endregion

        /// <summary>
        /// In Awake() we can set up things that dont require network data
        /// </summary>
        public virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            TargetDestination = GameObject.Find("TestTargetCube");
        }

        // Start is called before the first frame update
        public virtual void Start()
        {
            if (TargetDestination != null)
            {
                agent.SetDestination(TargetDestination.transform.position);
            }
        }


        // Update is called once per frame
        void Update()
        {

        }


        // To DO add Network.Destroy()
        private void OnDestroy()
        {

        }
    }

}