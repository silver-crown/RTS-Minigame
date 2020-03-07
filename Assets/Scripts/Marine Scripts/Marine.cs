using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;

public class Marine : RTS.Actor
{

    public override void Awake()
    {
        base.Awake();
        MyFaction = Factions.Marine;
    }
    
    // Start is called before the first frame update
    public override void Start()
    {
        WorldInfo.Actors.Add(gameObject);
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
