using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using MoonSharp.Interpreter;

/// <summary>
/// Drones are used by the enemy AI to interact in the world
/// </summary>
public class Drone : MonoBehaviour
{
    /// <summary>
    /// Behavior Tree used by the drone for micro world behaviors
    /// </summary>
    BehaviorTree _behaviorTree;

    // Navigation
    NavMeshAgent _navMeshAgent;
    public GameObject _target;

    /// <summary>
    /// Current Health Points the drone has
    /// </summary>
    public int Heatlh { get; protected set; }


    /// <summary>
    /// How much Metall the drone is currecntly carrying
    /// </summary>
    public int AmountMetall{ get; protected set; }
    
    /// <summary>
    /// The max amount of resources a drone can carry
    /// </summary>
    public int MaxResources{ get; protected set; }

    /// <summary>
    /// Keeps track if the drone has room for more resrouces or is full.
    /// </summary>
    public bool IsInventoryFull { get; protected set; }


    // we should probbly init drones with LUA or Scriptable objects
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// 
    /// </summary>
    public void MoonSharpReadStats()
    {/*
        string luaCode = @"    
		-- defines a factorial function
		function fact (n)
			if (n == 0) then
				return 1
			else
				return n*fact(n - 1)
			end
		end

		return fact(mynumber)";


        Script luaScript = new Script();
        DynValue result = luaScript.DoString(luaCode); */
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // _navMeshAgent.SetDestination(_target.transform.position);       
    }
}
