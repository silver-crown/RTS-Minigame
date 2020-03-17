using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using UnityEngine.Events;
using MoonSharp.Interpreter;
using Bbbt;
using UnityEditor;
using System;
using ssuai;
using RTS.Lua;

namespace RTS
{
    /// <summary>
    /// Base class used by drones for sight, movement etc.
    /// </summary>
    [RequireComponent(typeof(MouseClickRaycastTarget))]
    public abstract class Actor : MonoBehaviour
    {
        /// <summary>
        /// Contains the actor's lua script and table.
        /// </summary>
        protected LuaObject _luaObject;

        /// <summary>
        /// Whether the actor is selected.
        /// </summary>
        private bool _isSelected = false;

        /// <summary>
        /// The actor's type.
        /// </summary>
        public string Type { get; protected set; }

        #region Events
        /// <summary>
        /// The actor's MouseClickRaycastTarget.
        /// </summary>
        protected MouseClickRaycastTarget _mouseClickRaycastTarget = null;

        /// <summary>
        /// Invoked when an actor spawns.
        /// </summary>
        public static Action<Actor> OnActorSpawned = null;

        /// <summary>
        /// Invoked when the Actor gets clicked on.
        /// </summary>
        public static Action<Actor> OnActorClicked = null;
        #endregion

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
        /// The muzzle is the end of the gun, from where the projectile will be shot
        /// </summary>
        [SerializeField] public Transform GunEnd;

        /// <summary>
        /// How far the Actor can attack
        /// </summary>
        public float AttackRange { get; protected set; } = 10.0f;

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
        public WaitForSeconds ShotDuration = new WaitForSeconds(0.6f);

        /// <summary>
        /// 
        /// </summary>
        public float NextFire;

        #endregion

        #region AI
        /// <summary>
        /// The behaviour tree used for the Actor
        /// </summary>
        [SerializeField] BbbtBehaviourTree _behavior = null;

        /// <summary>
        /// The actor's NavMeshAgent. Used for making the actor move.
        /// </summary>

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

        /// <summary>
        /// Speciacations of the actor class determines how to attack
        /// </summary>
        public virtual void Attack()
        {
            Target.GetComponent<Actor>().TakeDamage((int)GetValue("_damage").Number);
            Debug.DrawLine(transform.position, Target.transform.position, Color.red, 0.15f);
        }

        /// <summary>
        /// Causes the actor to take damage.
        /// </summary>
        /// <param name="damage">The damage taken.</param>
        public virtual void TakeDamage(int damage)
        {
            SetValue("_hp", DynValue.NewNumber((int)GetValue("_hp").Number - damage));
            if ((int)_luaObject.Table.Get("_hp").Number <= 0)
            {
                Destroy(gameObject);
            }
        }


        /// <summary>
        /// Draws the laser line
        /// </summary>
        /// <returns></returns>
        protected IEnumerator ShootLaser()
        {
            // _gunAudio.Play();
            //LaserLine.enabled = true;
            yield return ShotDuration;
            //LaserLine.enabled = false;
        }


        public virtual void Awake()
        {
            WorldInfo.Actors.Add(this);

            if (GunEnd == null)
            {
                Debug.LogError(name + ":  GunEnd was null. Set Gun End in the inspector.", this);
            }

            _mouseClickRaycastTarget = GetComponent<MouseClickRaycastTarget>();
            if (_mouseClickRaycastTarget != null)
            {
                _mouseClickRaycastTarget.OnClick += () =>
                {
                    _isSelected = !_isSelected;
                    if (_isSelected)
                    {
                        OnActorClicked?.Invoke(this);
                    }
                };
            }
            else
            {
                Debug.LogError(name + ": No MouseClickRaycastTarget component.");
            }
        }

        // Start is called before the first frame update
        public virtual void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            if (TargetDestination != null)
            {
                agent.SetDestination(TargetDestination.transform.position);
            }
            
            _gunAudio = GetComponent<AudioSource>();

            OnActorSpawned?.Invoke(this);
        }

        // Update is called once per frame
        public virtual void Update()
        {
            var update = _luaObject.Script.Globals.Get("Update");
            if (update.IsNotNil())
            {
                _luaObject.Script.Call(update, _luaObject.Id);
            }
        }

        private void OnDestroy()
        {
            WorldInfo.Actors.Remove(this);
            if (this as Marine != null) WorldInfo.Marines.Remove(gameObject);
        }



        /// <summary>
        /// Gets a value from the Actor's table.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value associated with the key.</returns>
        public DynValue GetValue(string key)
        {
            return _luaObject.Table.Get(key);
        }

        /// <summary>
        /// Sets a value in the Actor's table.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>Sets the value associated with the key.</returns>
        public void SetValue(string key, DynValue value)
        {
            _luaObject.Table.Set(key, value);
        }

        /// <summary>
        /// Returns every pair in the actor's table.
        /// </summary>
        /// <returns>The pairs.</returns>
        public IEnumerable<TablePair> GetTablePairs()
        {
            return _luaObject.Table.Pairs;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_isSelected)
            {
                // Draw sight/attack range.
                Handles.color = Color.red;
                Handles.DrawWireDisc(transform.position, Vector3.up, (float)GetValue("_attackRange").Number);

                Handles.color = Color.green;
                Handles.DrawWireDisc(transform.position, Vector3.up, (float)GetValue("_sightRange").Number);

                // Highlight actors in sight/attack range.
                foreach (var actor in WorldInfo.Actors)
                {
                    if (actor == gameObject) continue;
                    float distance = Vector3.Distance(transform.position, actor.transform.position);
                    if (distance < GetValue("_attackRange").Number)
                    {
                        Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.2f);
                        Handles.DrawSolidDisc(actor.transform.position, Vector3.up, 1.3f);
                        //Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.4f);
                        //Gizmos.DrawSphere(actor.transform.position, 1.3f);
                    }
                    else if (distance < _luaObject.Table.Get("_sightRange").Number)
                    {
                        Handles.color = new Color(0.0f, 1.0f, 0.0f, 0.2f);
                        Handles.DrawSolidDisc(actor.transform.position, Vector3.up, 1.3f);
                        //Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.4f);
                        //Gizmos.DrawSphere(actor.transform.position, 1.3f);
                    }
                }

                // Visualise chunk scouting attractiveness
                var lastTimeScouted = GetValue("_lastTimeChunkWasScouted");
                if (lastTimeScouted.IsNotNil())
                {
                    foreach (var chunk in WorldInfo.Chunks)
                    {
                        var factor = new ChunkScoutingAttractiveness(chunk, this);
                        factor.UpdateUtility();
                        var utility = factor.GetUtility();
                        float r = utility;
                        float g = 1.0f - utility;
                        float b = 0.0f;
                        //if (chunk.x == 1 && chunk.y == 1) Debug.Log(time + ", " + r + ", " + b);
                        Handles.DrawSolidRectangleWithOutline(
                            new Vector3[]
                            {
                                new Vector3(chunk.x,        0.0f, chunk.y),
                                new Vector3(chunk.x + 1.0f, 0.0f, chunk.y),
                                new Vector3(chunk.x + 1.0f, 0.0f, chunk.y + 1.0f),
                                new Vector3(chunk.x,        0.0f, chunk.y + 1.0f)
                            },
                            new Color(r, g, b, 0.4f),
                            new Color(r, g, b, 1.0f)
                        );
                    }
                }
            }
        }
#endif
    }
}