using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Timers;


public class BehaviorTree : MonoBehaviour
{
    Behavior _rootNode; // root of the tree
    private static System.Timers.Timer aTimer;

    private static int _updateFrequency = 200; // milliseconds


    void Awake()
    {
        SetTimer();
    }

    private static void SetTimer()
    {
        // Create a timer with a two second interval.
        aTimer = new System.Timers.Timer(_updateFrequency);
        // Hook up the Elapsed event for the timer. 
        aTimer.Elapsed += Tick;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;

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

    // Update this 5hz
    private static void Tick(System.Object source, ElapsedEventArgs e)
    {
        // Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
        //                  e.SignalTime);

        var sigTime = e.SignalTime.ToString();

        Debug.Log("The Elapsed event was raised at {0:HH:mm:ss.fff}");
        Debug.Log(sigTime);

    }



    public void TerminateBehaviorTree()
    {
        // Deletes Timer
        aTimer.Stop();
        aTimer.Dispose();

    }
}
