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

        /// <summary>
        /// How far the actor can look
        /// </summary>
        public float LineOfSight = 30.0f;

        #endregion Vision

        #region Movement

        [Header("Movement")]
        public GameObject[] Waypoints;
        public GameObject TargetDestination;
        private NavMeshAgent agent;

        #endregion

        #region Personalisation

        /// <summary>
        /// A personal name that can be used for drones or marines. Note that this is 
        /// distinct from drone ID's.
        /// </summary>
        public string Name { get => name; }

        /// <summary>
        /// The faction the actor is alligned with
        /// </summary>
        public Factions Faction;

        #endregion

        public virtual void ReadStatsFromFile()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();
            agent.SetDestination(TargetDestination.transform.position);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}