using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

namespace RTS.Networking
{
    /// <summary>
    /// 
    /// </summary>
    public class SpawnNetworkedDrones : NetworkBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        public NetworkIdentity DronePrefab;

        /// <summary>
        /// The location where drones will be spawned.
        /// </summary>
        [SerializeField]
        public List<GameObject> DroneSpawnLocations;

        /// <summary>
        /// 
        /// </summary>
        void SpawnNetworkedDrone()
        {
           DroneStaticMethods.Create("FighterDrone", 0, 0, 0);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}