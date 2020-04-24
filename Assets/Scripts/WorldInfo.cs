﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using RTS;

/// <summary>
/// Stores references to all the actors, drones, resources, and other useful world info.
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
    /// List of all enterable buildings in the game.
    /// </summary>
    public static List<Vector2Int> Chunks { get; private set; }
    /// <summary>
    /// List of all enterable buildings in the game.
    /// </summary>
    public static List<EnterableBuilding> EnterableBuildings { get; private set; }
    /// <summary>
    /// List of all actors in the game.
    /// </summary>
    public static List<Actor> Actors { get; private set; }
    /// <summary>
    /// List of all the marines in the game. 
    /// </summary>
    public static List<GameObject> Marines { get; private set; }   
    /// <summary>
    /// List of all resources in the game.
    /// </summary>
    public static List<GameObject> Resources { get; private set; }
    /// <summary>
    /// List of all buildings that can hold resources in the game.
    /// </summary>
    public static List<GameObject> Depots { get; private set; }
    /// <summary>
    /// List of all factories in the game
    /// </summary>
    public static List<GameObject> Factories { get; private set; }

    static WorldInfo()
    {
        DroneTypes = new List<string>();
        ResourceTypes = new List<string>();
        Chunks = new List<Vector2Int>();
        EnterableBuildings = new List<EnterableBuilding>();
        Actors = new List<Actor>();
        Marines = new List<GameObject>();
        Resources = new List<GameObject>();
        Depots = new List<GameObject>();
        Factories = new List<GameObject>();

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

        // Populate the Chunks list with an arbitraty amount of chunks.
        // TODO: We want to be able to define chunk count in a reasonable manner.
        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 100; y++)
            {
                Chunks.Add(new Vector2Int(x, y));
            }
        }
    }
}