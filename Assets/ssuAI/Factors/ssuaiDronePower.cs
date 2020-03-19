using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using org.mariuszgromada.math.mxparser;

namespace ssuai
{
    public class ssuaiDronePower : Factor
    {
        CentralIntelligence _centralIntelligence;
        int _ID;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CI"> the central intelligence that controls the drones</param>
        /// <param name="function">the function string </param>
        public ssuaiDronePower(CentralIntelligence CI, int ID, string function)
        {
            MathFunction = new Function(function);
            _centralIntelligence = CI;
            _ID = ID;
        }

        public override void UpdateUtility()
        {
            //if we have some of the resource, calculate utility
            if (_centralIntelligence.GetDrone(_ID))
            {
                //I have absolutely no idea if this is gonna work
                _utility = (float)MathFunction.calculate(_centralIntelligence.GetDrone(_ID).powerLevel);
            }
            else //otherwise, well, we really need some.
            {
                _utility = 1.0f;
            }
        }
    }

}
