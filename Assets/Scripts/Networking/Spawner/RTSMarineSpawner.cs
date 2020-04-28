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
    /// When the client joins it spawns the players starting marines
    /// </summary>
    public override void OnStartClient()
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
            CmdSpawnMarine(StartingMarineSpawnLocations[i].transform.position);
        }
    }

    public override void OnStartLocalPlayer()
    {
        Debug.Log("Marines could have spawned now");
    }

    /// <summary>
    /// Used to spawn new marines
    /// </summary>
    /// <param name="pos">Position of the map to spawn marine</param>
    public void CmdSpawnMarine(Vector3 pos)
    {
        GameObject newMarine = Instantiate(MarinePrefab.gameObject, pos, Quaternion.identity);
        NetworkServer.Spawn(newMarine, connectionToClient);
    }
}
