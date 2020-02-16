using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* FROM BTSK article
 *  a Monitor
    node can be thought of as a parallel behavior with two sub-trees; one containing conditions which express 
    the assumptions to be monitored (read-only), and the other tree of
    behaviors (read–write). 

    Separating the conditions in one branch from the behaviors in the
    other prevents synchronization and contention problems, since only one sub-tree will be
    running actions that make changes in the world.
                                                                            */

// Class is named BTMonitor since unity alreday has a Monitor class
public class BTMonitor : Parallel
{

    BTMonitor(Policy success, Policy failure) : base(success, failure)
    {

    }

    public void AddCondition(Behavior condition)
    {

    }

    public void AddAction(Behavior action)
    {

    }
}
