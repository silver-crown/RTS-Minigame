using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;


namespace RTS.Networking
{

    /// <summary>
    ///  Spawnes networked game objects on the server
    /// </summary>
    public class RTSSpawner : NetworkBehaviour
    {
        [SerializeField]
        public NetworkIdentity dronePrefab;

        public override void OnStartServer()
        {
            // init spawns here
        }


        /// <summary>
        ///  Spawns a drone
        /// </summary>
        void SpawnDrone()
        {
            Vector3 spawnPosition = new Vector3(0, 0, 0);
            GameObject drone = Instantiate(dronePrefab.gameObject, spawnPosition, Quaternion.identity);

            NetworkServer.Spawn(drone);
        }

    }
}