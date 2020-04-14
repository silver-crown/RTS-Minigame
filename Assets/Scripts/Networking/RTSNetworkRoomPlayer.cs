
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

namespace RTS.Networking
{
    /// <summary>
    /// This class will be used to represent the player in the game lobby
    /// </summary>
    [AddComponentMenu("")]
    public class RTSNetworkRoomPlayer : NetworkRoomPlayer
    {
        public override void OnStartClient()
        {
            Debug.Log("Client Started");

            base.OnStartClient();
        }

        public override void OnClientEnterRoom()
        {
            Debug.Log("Client Enterd Room");
        }

        public override void OnClientExitRoom()
        {
            Debug.Log("Client Exited Room");
        }

    }

}