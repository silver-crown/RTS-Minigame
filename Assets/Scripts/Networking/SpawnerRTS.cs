using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

namespace RTS.Networking
{
    /// <summary>
    ///  Spawnes networked game objects on the server
    /// </summary>
    public class SpawnerRTS : NetworkBehaviour
    {
        [SerializeField]
        public NetworkIdentity playerUnitPrefab;

        /// <summary>
        /// The location where marines will be spawned.
        /// </summary>
        [SerializeField]
        public List<GameObject> marineSpawnLocations;

        [SerializeField]
        public int MarineStartingUnits = 1;

        public override void OnStartServer()
        {
            // init spawns her
            /*
            for (int i = 0; i < MarineStartingUnits; i++)
            {
                Debug.Log("Spawned a marine");
                SpawnMarine();
            } */
        }

        /// <summary>
        ///  Spawns a marine
        /// </summary>
        void SpawnMarine()
        {
            Debug.Log("Spawned a marine");

            Vector3 spawnPosition = new Vector3(0, 0, 0);
            GameObject newMarineUnit = Instantiate(playerUnitPrefab.gameObject, spawnPosition, Quaternion.identity);
            NetworkServer.Spawn(newMarineUnit);
        }
    }
}