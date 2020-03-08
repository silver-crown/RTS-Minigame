using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RTS
{
    /// <summary>
    /// Range weapon that can be attached to actors, giving them the ability to shoot.
    /// </summary>
    public class Gun : MonoBehaviour
    {
        // The weapons "owner"
        private RTS.Actor _actor;

        public void Shoot()
        {
        }

        public void Awake()
        {
            if(_actor == null)
            {
                _actor.GetComponent<Actor>();
            }
        }

    }
}