using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// This class is used to store the locaiton of all the player marines, drones and resources
/// inn the game world.
/// </summary>
public static class WorldInfo
{
    /// <summary>
    /// List of all the drone types in the game.
    /// </summary>
    public static List<string> DroneTypes { get; private set; }
    /// <summary>
    /// List of all the resource types in the game.
    /// </summary>
    public static List<string> ResourceTypes { get; private set; }
    /// <summary>
    /// List of all actors in the game.
    /// </summary>
    public static List<GameObject> Actors { get; private set; }
    /// <summary>
    /// List of all resources in the game.
    /// </summary>
    public static List<GameObject> Resources { get; private set; }

    static WorldInfo()
    {
        DroneTypes = new List<string>();
        ResourceTypes = new List<string>();
        Actors = new List<GameObject>();
        Resources = new List<GameObject>();

        // Read all drone types.
        var droneFiles = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "Lua", "Actors", "Drones"));
        Debug.Log("Loading drone types...");
        foreach (var file in droneFiles)
        {
            if (file.EndsWith(".lua"))
            {
                string type = Path.GetFileNameWithoutExtension(file);
                DroneTypes.Add(Path.GetFileNameWithoutExtension(file));
                Debug.Log("\t" + type);
            }
        }
        // Read all resource types.
        var resourceFiles = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "Lua", "Resources"));
        Debug.Log("Loading drone types...");
        foreach (var file in resourceFiles)
        {
            if (file.EndsWith(".lua"))
            {
                string type = Path.GetFileNameWithoutExtension(file);
                ResourceTypes.Add(Path.GetFileNameWithoutExtension(file));
                Debug.Log("\t" + type);
            }
        }
    }
}