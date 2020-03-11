using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

using MoonSharp.Interpreter;
using Bbbt;

namespace RTS
{

    /// <summary>
    /// Specialiced type of drone that specelises on combat
    /// </summary>
    public class CombatDrone : Drone
    {
        /// <summary>
        /// Set ups the behavior tree for the combat drone
        /// </summary>
        public void SetUpCombatBT()
        {
            MyBbbtBehaviourTreeComponent = GetComponent<BbbtBehaviourTreeComponent>();
            MyBbbtBehaviourTreeComponent.SetBehaviourTree("CombatDroneBT");
        }

        /// <summary>
        /// Reads the drone's stats from lua.
        /// </summary>
        override
        public void SetDroneType()
        {
            base.SetDroneType();

            Script script = new Script();
            script.DoFile("Actors\\Drones\\FighterDrone.lua");
            // AttackRange = (int)script.Globals.Get("attackRange").Number;
            // Debug.Log("Fighter Drone attack Range: " + AttackRange);
        }

        public override void Awake()
        {
            base.Awake();
            SetUpCombatBT();
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

        /// <summary>
        /// Shoots A laser form the combat drone to its target.
        /// </summary>
        public override void Attack()
        {
            if (Time.time > NextFire)
            {
                Debug.Log("Firing my LASER");
                NextFire = Time.time + FireRate;

                StartCoroutine(ShootLaser());

                LaserLine.SetPosition(0, _gunEnd.position);

                RaycastHit[] hits;
                hits = Physics.RaycastAll(_gunEnd.position,      // The starting point of the ray in world coordinates.
                                            transform.forward,  // The direction of the ray.
                                            AttackRange);       // The max distance the rayhit is allowed to be from the start of the ray.
                // https://learn.unity.com/tutorial/let-s-try-shooting-with-raycasts#5c7f8528edbc2a002053b468
            }
        }
    }
}