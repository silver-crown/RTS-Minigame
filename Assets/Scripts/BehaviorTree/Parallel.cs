using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Policy
{
    RequireOne,
    RequireAll,
};

public class Parallel : Composite
{
    private Policy _successPolicy;
    private Policy _failurePolicy;

    public Parallel(Policy success, Policy failure)
    {
        _successPolicy = success;
        _failurePolicy = failure;
    }

    override
    protected Status UpdateBehavior()
    {
        int successCount = 0;
        int failureCount = 0;

        for (int i = 0; i < _children.Count; i++)
        {
            Status childStatus;

            if (!_children[i].IsTerminated())
            {
               childStatus = _children[i].tick();

                if (childStatus == Status.SUCCESS)
                {
                    successCount++;
                    if(_successPolicy == Policy.RequireOne)
                    {
                        return Status.SUCCESS;
                    }
                }

                if(childStatus == Status.FAILURE)
                {
                    failureCount++;
                    if(_failurePolicy == Policy.RequireOne)
                    {
                        return Status.FAILURE;
                    }
                }

            }
 
        }

        if(_failurePolicy == Policy.RequireAll && failureCount == _children.Count)
        {
            return Status.FAILURE;
        }


        if(_successPolicy == Policy.RequireAll && successCount == _children.Count)
        {
            return Status.SUCCESS;
        }

        // Not sure what status to return here yet.
        return Status.INVALID;
    }


    // Makes sure the other behaviors that are running are terminated after the
    // Paralell has fufilled one of it's policy's.
    override
    protected  void OnTerminate(Status s)
    {
        for(int i = 0; i < _children.Count; i++)
        {
            if (_children[i].IsRunning())
            {
                _children[i].Abort();
            }
        }s
    }
}


