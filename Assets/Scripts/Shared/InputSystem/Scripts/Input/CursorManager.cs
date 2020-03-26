using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace Progress.InputSystem
{
    public class CursorManager : MonoBehaviour
    {
        private int playerId;
        public Player player { get; private set; }

        private void Awake()
        {
            
        }

        private void Start()
        {
            playerId = 0;
            player = ReInput.players.GetPlayer(playerId);
        }

        private void Update()
        {
            
        }
    }
}
