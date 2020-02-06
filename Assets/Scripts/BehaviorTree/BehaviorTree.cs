using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    Behavior _rootNode;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Update every 5Hz
    }

    // traverse the tree, but we have the update function
    public void tick()
    {
        // Update this 5hz
    }
}
