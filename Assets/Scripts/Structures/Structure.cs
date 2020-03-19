using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System.IO;

public class Structure : MonoBehaviour
{

    public int MaxHP { get; protected set; }
    public int HP { get; protected set; }

    [Tooltip("The file to read stats from")]
    [SerializeField]
    private string _luaFile;

    public string BuildingType { get; protected set; } = "UNSETBUILDINGNAME";   //TODO fix this

    //What do you do
    //Can be built
    //has a resource cost
    //
    //is added to a list of buildings
    //has a unique identifier
    //has

    private void Awake()
    {
        
        //initialize data
        _readStatsFromFile();

        //If I have an inventory add me to the list of buildings with inventories
        if (gameObject.GetComponent<Inventory>() != null)
        {
            WorldInfo.Depots.Add(this.gameObject);
        }
    }

    private void Update()
    {
        //if dead die
        if (HP == 0)
        {
            Die();
        }
    }

    private void _readStatsFromFile()
    {
        //open lua file
        Script script = new Script();
        var buildingTable = script.DoFile(_luaFile).Table;
        HP = (int)buildingTable.Get("_hp").Number;
        MaxHP = (int)buildingTable.Get("_hp").Number;


    }

    public void Die()
    {
        //remove resource from global resource list
        WorldInfo.Resources.Remove(gameObject);

        //destroy the gameobject
        Destroy(gameObject);
    }
}
