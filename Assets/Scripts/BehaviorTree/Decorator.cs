using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : Behavior
{
    protected Behavior _childNode;

    public Decorator(Behavior child)
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
