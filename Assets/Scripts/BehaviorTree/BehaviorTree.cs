using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Timers;


public class BehaviorTree : MonoBehaviour
{
    public Behavior _rootNode; // root of the tree
    private  System.Timers.Timer _timer;

    private  int _updateFrequency = 200; // milliseconds


    /*void Awake()
    {
        SetTimer();
    }*/

    public void SetTimer()
    {
        // Create a timer with a two second interval.
        _timer = new System.Timers.Timer(_updateFrequency);
        // Hook up the Elapsed event for the timer. 
        _timer.Elapsed += Tick;
        _timer.AutoReset = true;
        _timer.Enabled = true;

        Debug.Log("SetTimer called");
    }

    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {


    }

    /// <summary>
    ///  Updates the behavior tree of the central intelligence
    /// </summary>
    private void Tick(System.Object source, ElapsedEventArgs e)
    {
        var sigTime = e.SignalTime.ToString();
        //Debug.Log("Centreal Intelligence BT has ticked");
        //Debug.Log(sigTime);

        _rootNode.Tick();
    }

    public void TerminateBehaviorTree()
    {
        // Deletes Timer
        _timer.Stop();
        _timer.Dispose();
    }

    public void SetRootNode(Behavior in_rootNode)
    {
        _rootNode = in_rootNode;
    }

}
