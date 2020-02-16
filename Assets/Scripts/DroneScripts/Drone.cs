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
    BehaviorTree behaviorTree;
    public GameObject _target;
    NavMeshAgent _navMeshAgent;

    [SerializeField]
    private int _health;
    public int Health { get => _health; set { _health = value; } }

    [SerializeField] private int _resources;
    public int Resources   { get { return _resources; } set { _resources = value; }}

    [SerializeField] private int _maxResources; public int MaxResources {  get { return _maxResources; }
        set { _maxResources = value; }
    }

    // we should probbly init drones with LUA or Scriptable objects
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _health = 20;
        if (_health == 0)
        {
            _health = 20;
        }
        if (_resources == 0)
        {
            _resources = 0;
        }
        if (_maxResources == 0)
        {
            _maxResources = 6;
        }
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
        _navMeshAgent.SetDestination(_target.transform.position);
    }
}
