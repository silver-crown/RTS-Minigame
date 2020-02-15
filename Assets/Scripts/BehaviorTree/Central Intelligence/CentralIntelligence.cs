using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralIntelligence : MonoBehaviour
{
    // List of drone types
    BehaviorTree _behaviorTree = new BehaviorTree();

    // Need a counter for how many drones are doing what type of actions (Logging this could aslo help for
    // adding learning to the AI later).
 
    // Start is called before the first frame update
    void Start()
    {
        SetUpTreeFromCode();
        _behaviorTree.SetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Creates a behavior tree for the Centreal Intelligence base on a pre defined code
    /// </summary>
    public void SetUpTreeFromCode()
    {
        Selector rootNode = new Selector();
        CollectResources collectResources = new CollectResources();
        rootNode.AddChild(collectResources);
        
        _behaviorTree.SetRootNode(rootNode); // Creating the root node of the tree

    }


    /// <summary>
    /// Creates a behavior tree from a file.
    /// </summary>
    public bool SetUpBehaviorTreeFromFile()
    {
        return false;
    }

}


