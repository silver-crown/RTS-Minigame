using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Refactor this
public enum Status
{
    INVALID,
    SUCCESS,
    FAILURE,
    RUNNING,
    SUSPENDED,
}

public abstract class Behavior : MonoBehaviour
{
    private Status _status;

    // Constructor needs to be public so we can make subclasses of Behavior
    public Behavior()
    {
        _status = Status.INVALID;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tick();   
    }

    // Tick funciton updates the treee
    public Status tick()
    {
        if (_status != Status.RUNNING)
        {
            onInitialize();
        }

        _status = UpdateBehavior();

        if(_status != Status.RUNNING)
        {
            onTerminate(_status);
        }

        return _status;
    }

    // Called once before we call the update function of the behavior
    protected virtual void onInitialize()
    {

    }

    // called once each time the behavior tree is updated
    protected virtual Status UpdateBehavior()
    {   
        return 0;
    }

    // The onTerminate() method is called once, immediately after the previous
    // update signals it’s no longer running.
    protected virtual void onTerminate(Status s)
    {

    }
}
