using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using UnityEngine.Events;
using MoonSharp.Interpreter;
using Bbbt;

namespace RTS
    {

    /// <summary>
    /// Base class used by drones for sight, movement etc.
    /// </summary>
    public abstract class Actor : MonoBehaviour
    {
        #region Stats
        /// <summary>
        /// Current Health Points the drone has
        /// </summary>
        public int Health { get; protected set; }

        /// <summary>
        /// The amount of damge the actor deals.
        /// </summary>
        public int Damage { get; protected set; }

        /// <summary>
        /// How far the Actor can attack
        /// </summary>
        public int AttackRange { get; protected set; }


        #endregion

        #region AI
        /// <summary>
        /// The behaviour tree used for the Actor
        /// </summary>
        [SerializeField] BbbtBehaviourTree _behavior = null;

        /// <summary>
        /// Old Behavior Tree
        /// </summary>
        public BehaviorTree _behaviorTree;

        /// <summary>
        /// Add this script to the game object in unity
        /// </summary>
        public BbbtBehaviourTreeComponent MyBbbtBehaviourTreeComponent;

        #endregion AI

        #region Vision

        [Header("vision settings")]
        /// <summary>
        /// How far the actor can look, i.e it's view radius.
        /// </summary>
        [Tooltip("How big the character's view radius is")]
        [SerializeField]
        public float LineOfSight = 30.0f;

        /// <summary>
        /// If we can see an enemy
        /// </summary>
        public bool enemyInSight = false;

        /// <summary>
        /// inference, AI difficulty
        /// </summary>
        public Vector3 LastSighting;

        /// <summary>
        /// Used for the actor to access locations of other entites.
        /// </summary>
        public EntityLocations entityPosScript;

        #endregion Vision

        #region Movement

        [Header("Movement")]
        public GameObject[] Waypoints;
        public GameObject TargetDestination;
        public NavMeshAgent agent;

        #endregion

        #region Personalisation

        /// <summary>
        /// A personal name that can be used for drones or marines. Note that this is 
        /// distinct from drone ID's.
        /// </summary>
        public string Name { get => name; }

        [Header("Faction")]
        /// <summary>
        /// The faction the actor is alligned with
        /// </summary>
        public Factions MyFaction;

        #endregion

        public virtual void ReadStatsFromFile()
        {

        }

        public virtual void  Awake()
        {
            GameObject worldPos = GameObject.Find("WorldEntityLocationSystem");
            entityPosScript = worldPos.GetComponent<EntityLocations>();
        }

        // Start is called before the first frame update
        public virtual void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();

            if(TargetDestination != null)
            {
                agent.SetDestination(TargetDestination.transform.position);
            }
        }

        // Update is called once per frame
        public virtual void Update()
        {

        }
    }
}