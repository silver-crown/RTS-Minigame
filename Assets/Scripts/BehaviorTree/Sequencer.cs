using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : Composite
{
    // Start is called before the first frame update
    void Start()
    {

    }


    override
    protected Status UpdateBehavior()
    {
        for (int i = 0; i < _children.Count; i++)
        {

            Status childStatus = _children[i].Tick();

            if (childStatus == Status.RUNNING)
            {
                return Status.RUNNING;
            }
            else if (childStatus == Status.FAILURE)
            {
                return Status.FAILURE;
            }
        }

        return Status.SUCCESS;
    }

}
