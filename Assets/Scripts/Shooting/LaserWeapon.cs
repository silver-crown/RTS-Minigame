using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;

namespace RTS
{
    /// <summary>
    /// Range weapon that can be attached to actors, giving them the ability to shoot.
    /// </summary>
    public class LaserWeapon : MonoBehaviour
    {     
        /// <summary>
        /// Draws a straight line between points given to it.
        /// </summary>
        public LineRenderer LaserLine;

        // The weapons "owner"
        [SerializeField]
        public RTS.Actor _actor;

        public void Shoot()
        {

        }

        public void Awake()
        {
            if(_actor == null)
            {
                Debug.Log("No Owner set");
            }

            // The LineRenderer componenet is placed on the gun of the actor
            LaserLine = GetComponentInChildren<LineRenderer>();
        }

        /// <summary>
        /// Shoots A laser form the gunEnd of the owner to its owners target.
        /// </summary>
        private void ShootLaser()
        {
            if (Time.time > _actor.NextFire)
            {
                Debug.Log("Firing my LASER");
                Debug.Log("Fire rate is: " + _actor.FireRate);

                _actor.NextFire = Time.time + _actor.FireRate;

                StartCoroutine(ShootLaserCoroutine());

                LaserLine.SetPosition(0, _actor.GunEnd.position);

                // https://answers.unity.com/questions/763387/raycastall-find-closest-hit.html
                RaycastHit[] hits;
                hits = Physics.RaycastAll(_actor.GunEnd.position,     // The starting point of the ray in world coordinates.
                                            transform.forward,  // The direction of the ray.
                                            _actor.AttackRange);       // The max distance the rayhit is allowed to be from the start of the ray.

                LaserLine.SetPosition(1, _actor.Target.transform.position);

                float minDistance;
                int index = 0;

                Debug.Log("Hits lenght is: " + hits.Length);

                if (hits.Length > 0)
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


        /// <summary>
        /// Draws the laser line
        /// </summary>
        /// <returns></returns>
        protected IEnumerator ShootLaserCoroutine()
        {
            // _gunAudio.Play();
            LaserLine.enabled = true;
            yield return _actor.ShotDuration;
            LaserLine.enabled = false;

        }
    }

}