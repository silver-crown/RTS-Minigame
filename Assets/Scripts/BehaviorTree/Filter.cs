using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
   You can also easily create Boolean combinations of conditions to add to the filter, as a
   testimony to the power of core BT nodes like sequences (AND) and selectors (OR)             */
public class Filter : Sequence
{
    // might need a variable to keep track of how many conditions we have
    // The BT needs to make sure the conditions is first in the list so they are checked first.
    // private int _numConditions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddCondition(Behavior condition)
    {

    }

    public void AddAction(Behavior action)
    {

    }

}
