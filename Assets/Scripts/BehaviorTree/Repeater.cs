using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : Decorator
{
    protected int _limit;
    protected int _counter;


    public Repeater(BbbtBehaviour child) : base(child)
    {

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    override
    protected Status UpdateBehavior()
    {
         Status childStatus = _childNode.Tick();

        if (childStatus == Status.RUNNING)
        {
            // not sure if this if test is needed.
        }

        if (childStatus == Status.FAILURE)
        {
            return Status.FAILURE;
        }

        if (_counter == _limit)
        {
            return Status.SUCCESS;
        }

        return 0;
    }

    override
    protected void OnInitialize()
    {
        _counter = 0;
    }
}

