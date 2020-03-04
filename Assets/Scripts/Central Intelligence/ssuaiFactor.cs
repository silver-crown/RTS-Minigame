using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ssuai
{
    /// <summary>
    /// A Factor is some quantifiable element that affects the average utility of an Action.
    /// </summary>
    public abstract class Factor
    {
        //normalized utility value MUST be between 0 and 1.
        protected float _utility = 0.0f;

        /// <summary>
        /// Returns the utility value of the Factor
        /// </summary>
        /// <returns></returns>
        public abstract float GetUtility();

        /// <summary>
        /// Update the Factor's utility by whatever metric the derived class uses
        /// </summary>
        public abstract void UpdateUtility();

        /// <summary>
        /// Debug function - verify that the factor's utility is between 0.00 and 1.00. Should always be called before returning in GetUtility()
        /// </summary>
        protected void Validate()
        {
            Debug.Assert(_utility >= 0.0f || _utility <= 1.0f);
        }
    }
}
