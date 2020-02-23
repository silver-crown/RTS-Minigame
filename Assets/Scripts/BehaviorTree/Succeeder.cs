using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Succeeder : Decorator
{

    public Succeeder(BbbtBehaviour child) : base(child)
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    protected override Status UpdateBehavior()
    {
        //Returns Success for states Success/Failure, but will return RUNNING or the error states if necessary
        Status childStatus = _childNode.Tick();
        if (childStatus == Status.FAILURE)
        {
            return Status.SUCCESS;
        }
        else
        {
            return childStatus;
        }
    }
}
