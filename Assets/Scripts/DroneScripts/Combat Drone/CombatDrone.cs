using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

using MoonSharp.Interpreter;
using Bbbt;
using RTS;

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
    }

    /// <summary>
    /// Reads the drone's stats from lua.
    /// </summary>
    override
    public void SetDroneType()
    {
        base.SetDroneType();

        Script script = new Script();
        script.DoFile("Actors\\Drones\\FighterDrone.lua");
        // AttackRange = (int)script.Globals.Get("attackRange").Number;
        // Debug.Log("Fighter Drone attack Range: " + AttackRange);
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

    public override void Attack()
    {
     

    }

}