using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ssuai
{

    /// <summary>
    /// Utility function for the amount of a resource currently held, for the purpose of gathering more
    /// </summary>
    public class GatherResourceAmount : Factor
    {
        CentralIntelligence _centralIntelligence;
        string _resource;

        GatherResourceAmount (CentralIntelligence CI, string Resource)
        {
            _centralIntelligence = CI;
            _resource = Resource;
        }
        public override float GetUtility()
        {
            return _utility;
        }

        public override float UpdateUtility()
        {
            //Logistic curve that is high when the resource is near 0 and curves down when it approaches 100
            return (float)(1 / (1 + System.Math.Pow(System.Math.E, (_centralIntelligence.Resources[_resource]-50 / 10))));
        }
    }
}