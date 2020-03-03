using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ssuai
{

    /// <summary>
    /// Factor for the GatherResource action for the amount of a resource currently held, for the purpose of gathering more
    /// </summary>
    public class GatherResourceAmount : Factor
    {
        CentralIntelligence _centralIntelligence;
        string _resource;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CI">The central intelligence gathering the resources</param>
        /// <param name="Resource">The string representation (as in the resources dictionary) of the resource </param>
        public GatherResourceAmount(CentralIntelligence CI, string Resource)
        {
            _centralIntelligence = CI;
            _resource = Resource;
        }
        public override float GetUtility()
        {
            Validate();
            return _utility;
        }

        public override void UpdateUtility()
        {
            //Logistic curve that is high when the resource is near 0 and curves down when it approaches 100
            _utility = (float)(1 / (1 + System.Math.Pow(System.Math.E, (_centralIntelligence.Resources[_resource] - 50 / 10))));
        }
    }
}