using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

namespace RTS.Networking
{
    /// <summary>
    /// Marine class used for prototpying mirror networkin capabilites 
    /// for RTS game.
    /// </summary>
    public class NetworkedMarine : NetworkedActor
    {
        /// <summary>
        /// The highlight that will be used when unit is selected
        /// </summary>
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

        /// <summary>
        /// Tells the server to set the navmesh agents target destination.
        /// </summary>
        /// <param name="clickPoint">The coordinates of the new target</param>
        [Command]
        public void CmdMoveMarine(Vector3 clickPoint)
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