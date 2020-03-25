using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bbbt;
using System;
using org.mariuszgromada.math.mxparser;

namespace ssuai
{
    public class UtilityAction
    {
        protected List<Factor> _factors;
        public Action Behaviour { get; protected set; }
        private float _utility = 0.0f;


        /// <summary>
        /// Initializes the Utility action with the factors that affect its utility and the behavior it executes
        /// </summary>
        /// <param name="factors"></param>
        /// <param name="behavior"></param>
        public UtilityAction(List<Factor> factors, Action behaviour)
        {
            foreach (Factor factor in factors)
            {
                Debug.Log(factor);
                Debug.Log(factor.MathFunction);
                Debug.Log(factor.MathFunction.getFunctionExpressionString());
                //verify everything was read successfully.
                if (factor.MathFunction.getFunctionExpressionString() == "")
                {
                    Debug.LogError("Factor no. " + factors.IndexOf(factor) + ", is empty, is the Lua value read correctly?");
                }
            }
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