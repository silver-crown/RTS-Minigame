using UnityEngine;
using Yeeter;

/// <summary>
/// A player-controlled actor.
/// </summary>
public class Marine : RTS.Actor
{
    public override void Awake()
    {
        base.Awake();
        MyFaction = Factions.Marine;
        _luaObject = gameObject.GetComponent<LuaObjectComponent>();
        _luaObject.Load("Actors.Marines.BaseMarine");

    }
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        WorldInfo.Marines.Add(gameObject);
    }

    // Update is called once per frame
    public void Update()
    {
        // Marine moves towards combat drone. For testing purposes.
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
