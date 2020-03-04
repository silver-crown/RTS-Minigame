﻿using System.Collections;
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
        #region Combat

        // 1. Base Stats

        /// <summary>
        /// Current Health Points the drone has
        /// </summary>
        public int Health { get; protected set; }

        // 2. Shooting

        /// <summary>
        /// How far the Actor can attack
        /// </summary>
        public int AttackRange { get; protected set; }

        /// <summary>
        /// The muzzle is the end of the gun, from where the projectile will be shot
        /// </summary>
        public Transform muzzle;

        /// <summary>
        /// The amount of damage the projectile deals to the target
        /// </summary>
         public float GunDamage { get; protected set; }

        /// <summary>
        /// How fast the weapon shoots projectiles
        /// </summary>
        public int FireRate { get; protected set; }

        /// <summary>
        /// The entity the script wants to shoot at
        /// </summary>
        public GameObject Target;

        /// <summary>
        /// The prefab of the bullet that will be spawned when the weapon is fired.
        /// </summary>
        public GameObject ProjectilePrefab;

        /// <summary>
        /// This bool indicates if an enemy is within attacking distance of the actor.
        /// </summary>
        public bool EnemyInRange { get; protected set; }
 
        #endregion

        #region AI
        /// <summary>
        /// The behaviour tree used for the Actor
        /// </summary>
        [SerializeField] BbbtBehaviourTree _behavior = null;

        /// <summary>
        /// Old Behavior Tree
        /// </summary>
        public BehaviorTree BehaviorTree;

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
        [SerializeField]
        public bool EnemyInSight = false;

        /// <summary>
        /// inference, AI difficulty
        /// </summary>
        public Vector3 LastSighting;

        /// <summary>
        /// Used for the actor to access locations of other entites.
        /// </summary>
        public WorldInfo entityPosScript;


        [SerializeField]
        public List<GameObject> VisibleEnemies;

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

        /// <summary>
        /// Speciacations of the actor class determines how to attack
        /// </summary>
        public virtual void Attack()
        {

        }

        public virtual void  Awake()
        {
            GameObject worldPos = GameObject.Find("WorldEntityLocationSystem");
            entityPosScript = worldPos.GetComponent<WorldInfo>();
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