﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bbbt;

namespace ssuai
{
    public class Action
    {
        protected List<Factor> _factors;
        public BbbtBehaviour Behaviour { get; protected set; }
        private float _utility = 0.0f;


        /// <summary>
        /// Initializes the Utility action with the factors that affect its utility and the behavior it executes
        /// </summary>
        /// <param name="factors"></param>
        /// <param name="behavior"></param>
        public Action(List<Factor> factors, BbbtBehaviour behaviour)
        {
            _factors = factors;
            Behaviour = behaviour;
        }

        //Updates utility to be the average of all its factors utilities
        private void _updateUtility()
        {
            //Sum all normalized utility values
            float sum = 0.0f;
            foreach (Factor factor in _factors)
            {
                factor.UpdateUtility();
                sum += factor.GetUtility();
            }
            _utility = sum / _factors.Count; //set the action's utility to the average of the factors' utility
        }

        public float GetUtility()
        {
            _updateUtility(); //update the action's utility value
            return _utility; //return it
        }

    }
}