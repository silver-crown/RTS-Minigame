using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

namespace RTS.Networking
{
    /// <summary>
    /// Custom networking manager
    /// </summary>
    public class NetworkManagerRTS : NetworkManager
    {
        /// <summary>
        /// The minimum amout of players needed before the 
        /// </summary>
        [SerializeField] private int _minPlayers;

        /// <summary>
        /// The scene attribute allows us to drag in secene refernce in the editor
        /// </summary>
        [Scene] [SerializeField] private string menuScene = "";


    }
}