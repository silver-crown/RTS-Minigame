﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : BbbtBehaviour
{
    protected BbbtBehaviour _childNode;

    public Decorator(BbbtBehaviour child)
    {
        _childNode = child;
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
