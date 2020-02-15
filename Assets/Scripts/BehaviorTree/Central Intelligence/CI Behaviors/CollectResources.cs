using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectResources : Behavior
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    override
    protected Status UpdateBehavior()
    {
        return Status.RUNNING;
    }


}
