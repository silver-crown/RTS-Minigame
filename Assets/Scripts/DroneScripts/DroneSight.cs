using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// EnemySight can bed dragged on top of a game object to spot enemy players
/// </summary>
public class DroneSight : MonoBehaviour
{
    #region variables

    [Header("vision settings")]

    /// <summary>
    /// the view radius of the drone.
    /// </summary>
    [Tooltip("How big the drone's view circle is")]
    [SerializeField]
    public float viewRadius = 30f;

    /// <summary>
    /// Last positon player was spotted in
    /// </summary>
    public bool playerInSight = false;

    /// <summary>
    /// inference, AI difficulty
    /// </summary>
    public Vector3 LastSighting;
    #endregion


    /// <summary>
    /// 
    /// </summary>
    EntityLocations entityPosScript;

    private void Awake()
    {
        GameObject worldPos = GameObject.Find("WorldEntityLocationSystem");
        entityPosScript = worldPos.GetComponent<EntityLocations>();
    }

    void Update()
    {
        // 1. Loop over enemies
        for (int i = 0; i < entityPosScript.PlayerLocaitons.Count; i++)
        {
            if (Vector3.Distance(this.transform.position, entityPosScript.PlayerLocaitons[i].transform.position) <= viewRadius)
            {
                // Inn here we have spotted a player
                Debug.Log("Spotted player!");

                // Records last sighting of the player
                LastSighting = entityPosScript.PlayerLocaitons[i].transform.position;

                // Let CI know about the spotted player?
            }
        }

    }

}