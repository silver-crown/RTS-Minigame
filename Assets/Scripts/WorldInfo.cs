using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to store the locaiton of all the player marines, drones and resources
/// inn the game world.
/// </summary>
public static class WorldInfo
{
    /// <summary>
    /// List of all actors in the game.
    /// </summary>
    public static List<GameObject> Actors { get; private set; } = new List<GameObject>();

    /// <summary>
    /// List of all resources in the game.
    /// </summary>
    public static List<GameObject> Resources { get; private set; } = new List<GameObject>();
}