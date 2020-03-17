using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using org.mariuszgromada.math.mxparser;


namespace ssuai
{

    /// <summary>
    /// Factor for the amount of a resource currently held.
    /// </summary>
    public class ResourceAmount : Factor
    {
        private CentralIntelligence _centralIntelligence;
        string _resource;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="CI">The central intelligence gathering the resources</param>
        /// <param name="Resource">The string representation (as in the resources dictionary) of the resource </param>
        /// <param name="function">The function string. Should only have one parameter, which will be the amount of the resource specified. </param>
        public ResourceAmount(CentralIntelligence CI, string resource, string function)
        {
            MathFunction = new Function(function);
            _centralIntelligence = CI;
            _resource = resource;
        }

        public override void UpdateUtility()
        {
            //if we have some of the resource, calculate utility
            if (_centralIntelligence.Resources.ContainsKey(_resource))
            {
                _utility = (float)MathFunction.calculate(_centralIntelligence.Resources[_resource]);
            }
            else //otherwise, well, we really need some.
            {
                _utility = 1.0f;
            }
        }
    }
}