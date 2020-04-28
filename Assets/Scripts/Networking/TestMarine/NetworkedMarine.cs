using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

namespace RTS.Networking
{
    public class NetworkedMarine : NetworkedActor
    {
        GameObject highLight;

        public override void Awake()
        {
            base.Awake();
            highLight = this.transform.Find("HighLight").gameObject;
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void MoveMarine(Vector3 clickPoint)
        {
            agent.destination = clickPoint;
        }

        public void ActiveHighLight()
        {
            highLight.SetActive(true);
        }

        public void DeActiveateHighLight()
        {
            highLight.SetActive(false);
        }
    }

}