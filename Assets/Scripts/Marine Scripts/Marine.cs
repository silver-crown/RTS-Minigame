using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;
using MoonSharp.Interpreter;
using UnityEngine.AI;
using RTS.Lua;

public class Marine : RTS.Actor
{

    public override void Awake()
    {
        base.Awake();
        MyFaction = Factions.Marine;
        _luaObject = LuaManager.CreateLuaObject("Actors/Marines/BaseMarine");
    }
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        WorldInfo.Marines.Add(gameObject);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        // Marine move towards combat drone. For testing purposes.
        foreach (var actor in WorldInfo.Actors)
        {
            if (actor.Type == "FighterDrone")
            {
                var dist = actor.transform.position - transform.position;
                var dir = dist.normalized;
                transform.position += dir * Time.deltaTime;
            }
        }
    }
}
