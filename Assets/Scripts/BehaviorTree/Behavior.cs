using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    INVALID,
    SUCCESS,
    FAILURE,
    RUNNING,
    SUSPENDED,
    ABORTED
}

public class Behavior : MonoBehaviour
{
    private Status _status { get; set; }

    public Behavior()
    {
        _status = Status.INVALID;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    public Status Tick()
    {
       
        if (_status != Status.RUNNING)
        {
            OnInitialize();
        }

        // update selve behavior interface callet
        _status = UpdateBehavior();

        if (_status != Status.RUNNING)
        {
            OnTerminate(_status);
        }

        return _status;
    }

    /* The onInitialize() method is called once, immediately before the first call to
       the behavior’s update method. */
    protected virtual void OnInitialize()
    {

    }

    /* The update() method is called exactly once each time the behavior tree is
       updated, until it signals it has terminated thanks to its return status. */
    protected virtual Status UpdateBehavior()
    {
        return 0;
    }

    /* The onTerminate() method is called once, immediately after the previous
       update signals it’s no longer running. */
    protected virtual void OnTerminate(Status s)
    {

    }

    // Utility functions
    public void Reset()
    {
        _status = Status.INVALID;
    }

    public void Abort()
    {
        OnTerminate(Status.ABORTED);
        _status = Status.ABORTED;
    }

    public bool IsTerminated()
    {
        return _status == Status.SUCCESS || _status == Status.FAILURE;
    }

    public bool IsRunning()
    {
        return _status == Status.RUNNING;
    }

    public Status GetStatus()
    {
        return _status;
    }
}
