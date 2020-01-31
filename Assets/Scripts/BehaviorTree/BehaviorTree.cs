using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BehaviorTree : MonoBehaviour
{
    // 
    List<Behavior> _behaviors;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // we might just Update instread of tick.
    // no need to update the tree evry frame.
    // Every tick is basically a tree traversal.
    public void tick()
    {    

    }
}
