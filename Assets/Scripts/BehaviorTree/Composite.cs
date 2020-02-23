using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Composite : Behavior
{
    protected List<Behavior> _children = new List<Behavior>();


    // Composite methods
    public void AddChild(Behavior _child)
    {
        _children.Add(_child);
    }

    public void RemoveChild(Behavior _child)
    {

    }

    public void ClearChildren()
    {

    }



}
