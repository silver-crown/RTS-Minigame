using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Composite : BbbtBehaviour
{
    protected List<BbbtBehaviour> _children = new List<BbbtBehaviour>();


    // Composite methods
    public void AddChild(BbbtBehaviour _child)
    {
        _children.Add(_child);
    }

    public void RemoveChild(BbbtBehaviour _child)
    {

    }

    public void ClearChildren()
    {

    }



}
