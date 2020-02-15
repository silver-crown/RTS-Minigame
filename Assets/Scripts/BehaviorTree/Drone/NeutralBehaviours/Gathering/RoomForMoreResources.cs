using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomForMoreResources : Behavior
{
    // [SerializeField] DroneStats _Stats;
    public bool isFull;
    // Start is called before the first frame update
    void Start()
    {
        isFull = false;
    }

    // Update is called once per frame
    void Update()
    {
       // if(_Stats.GetResources() < _Stats.GetResourceLimit())
        {
            isFull = true;
        }
    }
}
