using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneStats : MonoBehaviour
{
    //A script keeping track of a drone's health and statistics 

    [SerializeField]
    private int _health;
    public int Health { get => _health;  set { _health = value;  } } 


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


    // Start is called before the first frame update
    void Start()
    {
        if(_health == 0)
        {
            _health = 20;
        }
        if(_resources == 0)
        {
            _resources = 0;
        }
        if(_maxResources == 0)
        {
            _maxResources = 6;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public float GetHealth()
    {
        return _health;
    }
    public int GetResources()
    {
        return _resources;
    }
    public int GetResourceLimit()
    {
        return _maxResources;
    }
}
