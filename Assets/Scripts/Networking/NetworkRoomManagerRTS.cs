using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

namespace RTS.Networking
{
    /// <summary>
    /// Manages scenes and connected clients
    /// </summary>
    public class NetworkRoomManagerRTS : NetworkRoomManager
    {
        bool showStartButton;

        /// <summary>
        /// This method is called after GamePlayer Object is instantiated and just before it replaces RoomPlayer Object.
        /// This is where you pass data from the RoomPlayer to the GamePlayer object as it is about to enter the online scene.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="roomPlayer"></param>
        /// <param name="gamePlayer"></param>
        /// <returns>true unless code makes it abort </returns>
        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            return true;
        }

        /// <summary>
        /// Think this is called when all players are ready.
        /// </summary>
        public override void OnRoomServerPlayersReady()
        {
            if (isHeadless)
            {
                base.OnRoomServerPlayersReady();
            }
            else
            {
                showStartButton = true;
            }
        }


        public override void OnGUI()
        {
            base.OnGUI();

            if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
            {
                // set to false to hide it in the game scene
                showStartButton = false;

                ServerChangeScene(GameplayScene);
            }
        }
    }

}