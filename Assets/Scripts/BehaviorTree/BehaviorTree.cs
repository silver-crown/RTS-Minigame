using System.Timers;
using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    /// <summary>
    /// Root of the behavior tree
    /// </summary>
    public Behavior _rootNode; // root of the tree

    /// <summary>
    /// 
    /// </summary>
    private Timer _timer;

    /// <summary>
    /// How often the behavior tree will update.
    /// e.g a value of 200 here means it will update every 200 milliseconds.
    /// </summary>
    private  int _updateFrequency = 200; // milliseconds

    GameObject owner;

    void Awake()
    {
        SetTimer();
    }

    public void SetTimer()
    {
        // Create a timer with a two second interval.
        _timer = new Timer(_updateFrequency);
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

    /// <summary>
    ///  Ticks the behavior tree
    /// </summary>
    private void Tick(System.Object source, ElapsedEventArgs e)
    {
        var sigTime = e.SignalTime.ToString();
        //Debug.Log(" BT has ticked");
        //Debug.Log(sigTime);

        _rootNode.Tick();
    }

    /// <summary>
    ///  Terminates the behavior tree so it does not continue executing after the object is deleted.
    /// </summary>
    public void TerminateBehaviorTree()
    {
        // Deletes Timer
        _timer.Stop();
        _timer.Dispose();
    }

    /// <summary>
    /// Sets the root node of the behavior tree, this is where the tree starts its traversel.
    /// </summary>
    /// <param name="in_rootNode"></param>
    public void SetRootNode(Behavior in_rootNode)
    {
        _rootNode = in_rootNode;
    }

    /// <summary>
    /// Calls the TerminateBehavior function to clean up after deletion
    /// </summary>
    void OnDestroy()
    {
        TerminateBehaviorTree();
        Debug.Log("BT has been terminated.");
    }
}
