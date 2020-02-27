using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

using Bbbt;

/// <summary>
/// Specialiced type of drone that specelises on combat
/// </summary>
public class CombatDrone : Drone
{
    /// <summary>
    /// Set ups the behavior tree for the combat drone
    /// </summary>
    public void SetUpCombatBT()
    {
        MyBbbtBehaviourTreeComponent = GetComponent<BbbtBehaviourTreeComponent>();
        MyBbbtBehaviourTreeComponent.SetBehaviourTree("CombatDroneBT");


       /* // adds the root node
        Sequencer sequencer = new Sequencer();

        // First thing the combat drone checks is the findplayer behavior
        CanSeeEnemy canSeeEnemy = new CanSeeEnemy();
        sequencer.AddChild(canSeeEnemy);

        // Creates the behavior treee
        _behaviorTree = GetComponent<BehaviorTree>();
        _behaviorTree.SetRootNode(sequencer);*/

       
    }

    public override void Awake()
    {
        base.Awake();
        SetUpCombatBT();
    }

    // Start is called before the first frame update
    public override void  Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}