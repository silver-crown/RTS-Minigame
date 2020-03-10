using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using UnityEngine.Events;
using MoonSharp.Interpreter;
using Bbbt;
using UnityEditor;

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
        /// Current Health Points the actor has
        /// </summary>
        public int Health { get; protected set; } = 40;

        /// <summary>
        /// Max health points the actor has
        /// </summary>
        public int MaxHealth { get; protected set; } = 40;

        // 2. Shooting

        /// <summary>
        /// How far the Actor can attack
        /// </summary>
        public float AttackRange { get; protected set; } = 10.0f;

        /// <summary>
        /// The muzzle is the end of the gun, from where the projectile will be shot
        /// </summary>
        public Transform GunEnd;

        /// <summary>
        /// The amount of damage the projectile deals to the target
        /// </summary>
        public float GunDamage { get; protected set; }

        /// <summary>
        /// How fast the weapon shoots projectiles
        /// </summary>
        public float FireRate { get; protected set; }

        /// <summary>
        /// The current target of the actor, this is the enemy that the actor will attack
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

        /// <summary>
        /// How long the laser will be visible after it has been shot
        /// </summary>
        private WaitForSeconds ShotDuration = new WaitForSeconds(0.9f);

        /// <summary>
        /// Draws a straight line between points given to it.
        /// </summary>
        public LineRenderer LaserLine;

        /// <summary>
        /// 
        /// </summary>
        public float NextFire { get; protected set; }
 
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

        #region Audio

        /// <summary>
        /// 
        /// </summary>
        private AudioSource _gunAudio;

        #endregion

        public virtual void SetDroneType()
        {

        }

        /// <summary>
        /// Speciacations of the actor class determines how to attack
        /// </summary>
        public virtual void Attack()
        {

        }

        /// <summary>
        /// Draws the laser line
        /// </summary>
        /// <returns></returns>
        protected IEnumerator ShootLaser()
        {
            // _gunAudio.Play();
            LaserLine.enabled = true;
            yield return ShotDuration;
            LaserLine.enabled = false;

        }


        public virtual void  Awake()
        {
            WorldInfo.Actors.Add(gameObject);
        }

        // Start is called before the first frame update
        public virtual void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();

            if (TargetDestination != null)
            {
                agent.SetDestination(TargetDestination.transform.position);
            }

            LaserLine = GetComponent<LineRenderer>();
            _gunAudio = GetComponent<AudioSource>();

            GunEnd = GameObject.Find("GunEnd").transform;

        }

        // Update is called once per frame
        public virtual void Update()
        {

        }

        private void OnDrawGizmosSelected()
        {
            // Draw sight/attack range.
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.up, AttackRange);
            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, Vector3.up, LineOfSight);

            // Highlight actors in sight/attack range.
            foreach (var actor in WorldInfo.Actors)
            {
                if (actor == gameObject) continue;
                float distance = Vector3.Distance(transform.position, actor.transform.position);
                if (distance < AttackRange)
                {
                    Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.2f);
                    Handles.DrawSolidDisc(actor.transform.position, Vector3.up, 1.3f);
                    //Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.4f);
                    //Gizmos.DrawSphere(actor.transform.position, 1.3f);
                }
                else if (distance < LineOfSight)
                {
                    Handles.color = new Color(0.0f, 1.0f, 0.0f, 0.2f);
                    Handles.DrawSolidDisc(actor.transform.position, Vector3.up, 1.3f);
                    //Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.4f);
                    //Gizmos.DrawSphere(actor.transform.position, 1.3f);
                }
            }
        }
    }
}