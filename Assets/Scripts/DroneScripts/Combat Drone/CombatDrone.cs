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
            // FireRate = (int)script.Globals.Get("_fireRate").Number;
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
            FireRate = 1.0f;

            if (Time.time > NextFire)
            {
                Debug.Log("Firing my LASER");
                Debug.Log("Fire rate is: " + FireRate);

                NextFire = Time.time + FireRate;

                StartCoroutine(ShootLaser());

                LaserLine.SetPosition(0, _gunEnd.position);

                // https://answers.unity.com/questions/763387/raycastall-find-closest-hit.html
                RaycastHit[] hits;
                hits = Physics.RaycastAll(_gunEnd.position,     // The starting point of the ray in world coordinates.
                                            transform.forward,  // The direction of the ray.
                                            AttackRange);       // The max distance the rayhit is allowed to be from the start of the ray.

                LaserLine.SetPosition(1, Target.transform.position);

                float minDistance;
                int index = 0;

                Debug.Log("Hits lenght is: " + hits.Length);

                if(hits.Length > 0)
                {
                    if ((hits.Length == 1 && hits[0].transform == this.gameObject.transform))
                    {
                        Debug.Log("We shot our self, same as hitting nothing");
                    }
                    else
                    {
                        minDistance = hits[0].distance;

                        for (int i = 0; i < hits.Length; i++)
                        {
                            if (hits[i].distance < minDistance && hits[i].transform != this.gameObject.transform)
                            {
                                index = i;
                                minDistance = hits[i].distance;
                            }
                        }

                        //LaserLine.SetPosition(1, hits[index].point); 
                    }
                }
            }
        }
    }
}