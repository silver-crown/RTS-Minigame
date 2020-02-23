using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to store the locaiton of all the player marines and drones
/// inn the game world.
/// </summary>
public class EntityLocations : MonoBehaviour
{
    /// <summary>
    /// List of the player locations. i.e things the drones can spot
    /// </summary>
    [Tooltip("Player Marines")]
    [SerializeField]
    public List<GameObject> PlayerLocaitons = new List<GameObject>();


}