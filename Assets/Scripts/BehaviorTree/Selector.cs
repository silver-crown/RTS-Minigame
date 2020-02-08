using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Composite
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    override
    protected Status UpdateBehavior()
    {

        // loop over children. until one returns RUNNING or SUCCESS
        // if none of the children excutes it returns failure.
        while (true)
        {
            for (int i = 0; i < _children.Count; i++)
            {
                Status childStatus = _children[i].tick();

                // If the child is currecntly running or succseeds do it again.
                if(childStatus != Status.FAILURE)
                {
                    return childStatus;
                }
            }
            // None of the children was able to start RUNNING or SUCCSESS(fully) execute.
            return Status.INVALID; // the reason for invalid instead of failure is since tecnically 
                                   // a node did not fail, it's just no more nodes to check.
        }
    }


}
