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
        public NetworkIdentity playerUnitPrefab;

        public override void OnStartServer()
        {
            // init spawns here
            SpawnMarine();
        }


        /// <summary>
        ///  Spawns a drone
        /// </summary>
        void SpawnMarine()
        {
            Vector3 spawnPosition = new Vector3(0, 0, 0);
            GameObject drone = Instantiate(playerUnitPrefab.gameObject, spawnPosition, Quaternion.identity);

            NetworkServer.Spawn(drone);
        }

    }
}