using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoonSharp.Interpreter;
using Bbbt;
using RTS;
using Yeeter;


/// <summary>
/// Drones are used by the enemy AI/CI to interact in the world
/// </summary>
[MoonSharpUserData]
public class Drone : Actor
{
    /// <summary>
    /// The id to assign to the next instantiated drone.
    /// </summary>
    private static int _nextId = 0;

    /// <summary>
    /// Drone Group and it's ID
    /// </summary>
    [System.NonSerialized] public int groupID;

    /// <summary>
    /// If the drone is a leader or not
    /// </summary>
    [System.NonSerialized] public bool leaderStatus = false;
    [SerializeField] GroupLeader group;

    [SerializeField] public string TargetResourceType { get; private set; } = "Metal";

    /// <summary>
    /// The depot the drone is currently delivering resources to
    /// </summary>
    [SerializeField] public GameObject TargetDepot = null;

    public enum GroupUnit
    {
        Alpha,
        Bravo,
        Charlie,
        Delta
    }
    public GroupUnit myUnit;

    /// <summary>
    /// Unique ID of the drone
    /// </summary>
    public int ID { get; protected set; }

    /// <summary>
    /// How many enemies the Drone has destroyed
    /// </summary>
    public int killCount;

    /// <summary>
    /// The absolute strength of the drone
    /// </summary>
    public double powerLevel;
    public bool highlight;

    /// <summary>
    /// The drone's central intelligence.
    /// </summary>
    public CentralIntelligence CentralIntelligence { get; set; }


    public override void Awake()
    {
        base.Awake();

        CentralIntelligence = GameObject.Find("CI").GetComponent<CentralIntelligence>();

        ID = _nextId++;
    }

    public override void Start()
    {
        base.Start();
    }

    public void Update()
    {
        /* This code is here until some prerequsities get done, pls no touchy
        string status = GetValue("_status").String;

        //check if we recently switched into Idle mode
        //TODO replace with an Event once Benjamin implements that
        if (status != GetValue("_behaviourTree").String)
        {
            //if so set up the proper status tree
            GetComponent<BbbtBehaviourTreeComponent>().SetBehaviourTree(status);
        }

        */


        //Debug.Log(_script.Call(_script.Globals["Update"]));

        var lastTimeScouted = GetValue("_lastTimeChunkWasScouted");
        if (lastTimeScouted.IsNotNil())
        {
            // Update when the actor saw a chunk
            foreach (var chunk in WorldInfo.Chunks)
            {
                var rect = new Rect(chunk, Vector2.one);
                var pos = new Vector2(transform.position.x, transform.position.z);
                var distances = new float[]
                {
                    Vector2.Distance(pos, new Vector2(rect.xMin, rect.yMin)),
                    Vector2.Distance(pos, new Vector2(rect.xMin, rect.yMax)),
                    Vector2.Distance(pos, new Vector2(rect.xMax, rect.yMin)),
                    Vector2.Distance(pos, new Vector2(rect.xMax, rect.yMax))
                };

                bool inSightRange = true;
                foreach (var distance in distances)
                {
                    if (distance > GetValue("_sightRange").Number)
                    {
                        inSightRange = false;
                    }
                }

                if (inSightRange)
                {
                    lastTimeScouted.Table.Set(chunk.ToString(), DynValue.NewNumber(Time.time));
                    if (CentralIntelligence != null)
                    {
                        CentralIntelligence.SetLastTimeScouted(chunk, Time.time);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Makes the drone Attack its target
    /// </summary>
    public override void Attack()
    {
        base.Attack();
    }


    /// <summary>
    /// Reads the drone's stats from lua.
    /// </summary>
    /// <param name="type">The drone type to set </param>
    /// <param name="id">The drone's id.</param>
    //public void Initialize(string type, int id)
    //{
        // Why does this method exist? I'll just reroute it to SetType. Did I create this? I don't even know.
        // It doesn't even load behaviour trees properly?
        // - Tired and confused Benjamin
        //SetType(type);
        /*
        //GetComponent<NavMeshAgent>().
        Type = type;
        _luaObject = GetComponent<LuaObjectComponent>();
        if (_luaObject == null)
        {
            _luaObject = gameObject.AddComponent<LuaObjectComponent>();
            InGameDebug.Log(name + ": No LuaObjectComponent. Created one.");
        }
        _luaObject.Load("Actors.Drones." + type);
        string tree = GetValue("_behaviourTree").String;
        */
    //}

    /// <summary>
    /// Reads the drone's stats from lua.
    /// </summary>
    /// <param name="type">The drone type to set </param>
    public void SetType(string type)
    {
        Type = type;
        _luaObject = GetComponent<LuaObjectComponent>();
        if (_luaObject == null)
        {
            _luaObject = gameObject.AddComponent<LuaObjectComponent>();
        }
        _luaObject.Load("Actors.Drones." + type);
        string tree = GetValue("_behaviourTree").String;

        if (tree != null)
        {
            GetComponent<BbbtBehaviourTreeComponent>().SetBehaviourTree(tree);
        }
        else
        {
            Debug.LogError(GetType().Name + ".SetType(): _behaviourTree not present in " + type + ".lua", this);
        }

        InGameDebug.Log(Type + " boy reporting for duty.");
        name = ObjectBuilder.GetId(gameObject) + "_" + type;
    }


    protected void SetStatus(string status)
    {
        SetValue("_status", DynValue.NewString(status));
    }

    /// <summary>
    /// Changes the depot where drone will deliver resources.
    /// </summary>
    /// <param name="target">Depot to be targeted</param>
    public void SetTargetDepot(GameObject target)
    {
        TargetDepot = target.gameObject;
    }

    /// <summary>
    /// Set the group script's id to match that of the drone
    /// </summary>
    void SetupGroup()
    {
        group.groupID = groupID;
    }

    /// <summary>
    /// PowerLevel is a value that is calcualted to determine how strong individual 
    /// drones are compared to each other.
    /// </summary>
    public void CalculatePowerLevel()
    {
        double dps = GetValue("_attacksPerSecond").Number;
        double range = GetValue("_attackRange").Number;
        powerLevel = killCount + dps + range + Health;
    }

    /// <summary>
    /// Creates a drone of type 'type' at the specified location
    /// </summary>
    /// <param name="type">The drone type to be spawned</param>
    /// <param name="x">x spawn position of the drone</param>
    /// <param name="y">y spawn position of the drone</param>
    /// <param name="z">z spawn position of the drone</param>
    public static void Create(string type, float x = 0, float y = 0, float z = 0)
    {
        DroneStaticMethods.Create(type, x, y, z);
    }

    private void OnDrawGizmos()
    {
        if (highlight)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 1);
        }
    }
}
