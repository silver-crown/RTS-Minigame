using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class totalDrones : MonoBehaviour
{
    Dictionary<string, int> drones;

    ///Increase or decrease your drone total for a specific drone type
    public void IncreaseDrone(string s, int i)
    {
        drones[s] += i;
    }
    public void DecreaseDrone(string s, int i)
    {
        drones[s] -= i;
    }
    ///Gets the total of a specified drone type
    public int GetDroneTotal(string s)
    {
        return drones[s];
    }
}
