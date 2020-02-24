using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Faction enum is used to assign teams to characters
/// </summary>
public enum Factions
{
    Drone,
    HumanPlayer,
    Elders 
}

/// <summary>
/// This script is used to store the faction allignment of the character / object.
/// </summary>
public class Faction : MonoBehaviour
{
    /// <summary>
    /// The factions of that the owner object is alligned to.
    /// </summary>
    [SerializeField]
    public Factions myFaction;

}
