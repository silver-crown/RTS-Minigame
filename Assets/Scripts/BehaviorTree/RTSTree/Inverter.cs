﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Decorator
{

    public Inverter(Behavior child) : base(child)
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    protected override Status UpdateBehavior()
    {
        Status childStatus = _childNode.tick();

        if (childStatus == Status.SUCCESS)
        {
            return Status.FAILURE;
        } else if (childStatus == Status.FAILURE)
        {
            return Status.SUCCESS;
        } else
        {
            return childStatus;
        }
    }
}