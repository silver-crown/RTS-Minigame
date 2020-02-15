using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    BehaviorTree behaviorTree;

    [SerializeField]
    private int _health;
    public int Health { get => _health; set { _health = value; } }


    [SerializeField] private int _resources;
    public int Resources
    {
        get { return _resources; }
        set { _resources = value; }
    }

    [SerializeField] private int _maxResources;
    public int MaxResources
    {
        get { return _maxResources; }
        set { _maxResources = value; }
    }


    private void Awake()
    {
        // Set stats with LUA

        // 1. Read in LUA file
        // 2. set vars based on LUA

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
