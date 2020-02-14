using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralIntelligence : MonoBehaviour
{
    // List of drone types
    BehaviorTree _behaviorTree = new BehaviorTree();
 
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

    public void SetUpTreeFromCode()
    {
        _behaviorTree.SetRootNode(new Selector());
    }


    // Return values:
    // False: syntax erros etc in file
    // True: Succsesfully made a behavior tree from the file. 
    public bool SetUpBehaviorTreeFromFile()
    {
        return false;
    }

}


