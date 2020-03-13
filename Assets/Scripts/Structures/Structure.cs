using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public class Structure : MonoBehaviour
{

    public int MaxHP { get; protected set; }
    public int HP { get; protected set; }

    [Tooltip("The file to read stats from")]
    [SerializeField]
    private string _luaFile;

    public string BuildingType { get; protected set; }

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

        //add self to building list
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
        HP = (int)buildingTable.Get("HP").Number;
        MaxHP = (int)buildingTable.Get("HP").Number;

        BuildingType = script.Globals.Get("BuildingType").String;
    }

    public void Die()
    {
        //remove resource from global resource list
        WorldInfo.Resources.Remove(gameObject);

        //destroy the gameobject
        Destroy(gameObject);
    }
}
