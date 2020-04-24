using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

/// <summary>
/// Used to spawn Marines for the player
/// </summary>
public class RTSMarineSpawner : NetworkBehaviour 
{

    int MAX_NUMBER_START_MARINES = 3;

    /// <summary>
    /// The prefab of the marine
    /// </summary>
    [SerializeField]
    public NetworkIdentity MarinePrefab;

    /// <summary>
    /// The location where the marines will be spawned.
    /// </summary>
    [SerializeField]
    public List<GameObject> StartingMarineSpawnLocations;

    /// <summary>
    /// When the server starts it spawns the players starting marines
    /// </summary>
    public override void OnStartServer()
    {
        Debug.Log("Spawn Starting Marines");

        // Loops over the length of the Marine Spawn Locations and spawns Marines
        for(int i = 0; i < StartingMarineSpawnLocations.Count; i++)
        {
            // If ther are assigned more starting locations 
            // than the marine player is supposed to start with
            if(i >= MAX_NUMBER_START_MARINES)
            {
                break;
            }

            SpawnMarine(StartingMarineSpawnLocations[i].transform.position);

        }
    }

    // OnStartClient 

    /// <summary>
    /// Used to spawn new marines
    /// </summary>
    /// <param name="pos"></param>
    public void SpawnMarine(Vector3 pos)
    {
        GameObject newMarine = Instantiate(MarinePrefab.gameObject, pos, Quaternion.identity);
        NetworkServer.Spawn(newMarine);
    }
}
