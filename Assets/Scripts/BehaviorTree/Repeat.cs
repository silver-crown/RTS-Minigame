using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeat : Decorator
{
    protected int _limit;
    protected int _counter;


    public Repeat(Behavior child) : base(child)
    {

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    override
    protected Status UpdateBehavior()
    {
        _childNode.tick();

        if (_childNode.GetStatus() == Status.RUNNING)
        {
            // not sure if this if test is needed.
        }

        if (_childNode.GetStatus() == Status.FAILURE)
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

