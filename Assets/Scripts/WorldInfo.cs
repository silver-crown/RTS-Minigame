using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to store the locaiton of all the player marines, drones and resources
/// inn the game world.
/// </summary>
public class WorldInfo : MonoBehaviour
{
    public static WorldInfo Instance;
    /// <summary>
    /// List of the player locations. i.e things the drones can spot
    /// </summary>
    [Tooltip("Player Marines")]
    [SerializeField]
    public List<GameObject> MarineLocations = new List<GameObject>();

    [Tooltip("Resources")]
    [SerializeField]
    public List <GameObject> Resources = new List<GameObject>();

    private WorldInfo()
    {

    }

    private void Awake()
    {
        Instance = new WorldInfo();
    }
}