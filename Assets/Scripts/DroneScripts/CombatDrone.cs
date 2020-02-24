using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // adds the root node
        Sequencer sequencer = new Sequencer();

        // First thing the combat drone checks is the findplayer behavior
        CanSeeEnemy canSeeEnemy = new CanSeeEnemy();
        sequencer.AddChild(canSeeEnemy);

        // Creates the behavior treee
        _behaviorTree = GetComponent<BehaviorTree>();
        _behaviorTree.SetRootNode(sequencer);

    }

    private void Awake()
    {
        SetUpCombatBT();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}