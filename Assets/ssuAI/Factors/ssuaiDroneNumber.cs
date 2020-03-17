using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using org.mariuszgromada.math.mxparser;


namespace ssuai
{
    
    /// <summary>
    /// Factor for amount of drones. Currently just takes the totalt amount of drones, TODO expand to work with other drone types.
    /// </summary>
    public class DroneNumber : Factor
    {
        CentralIntelligence _centralIntelligence;

        public DroneNumber(CentralIntelligence CI, string function)
        {
            MathFunction = new Function(function);
            _centralIntelligence = CI;
        }

        public override void UpdateUtility()
        {
            _utility = (float)MathFunction.calculate(_centralIntelligence.DroneCount - CentralIntelligence.MAXDRONES / 2);
        }
    }
}