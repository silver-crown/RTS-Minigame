using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;

public class Marine : RTS.Actor
{

    public override void Awake()
    {
        base.Awake();

        EntityLocations.MarineLocations.Add(this.gameObject);
        MyFaction = Factions.Marine;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
