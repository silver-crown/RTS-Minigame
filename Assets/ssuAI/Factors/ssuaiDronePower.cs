using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using org.mariuszgromada.math.mxparser;

namespace ssuai
{
    public class DronePower : Factor
    {
        CentralIntelligence _centralIntelligence;
        int _ID;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CI"> the central intelligence that controls the drones</param>
        /// <param name="function">the function string </param>
        public DronePower(CentralIntelligence CI, int ID, string function)
        {
            MathFunction = new Function(function);
            _centralIntelligence = CI;
            _ID = ID;
        }

        public override void UpdateUtility()
        {
            //if the drone is in the list, calculate utility
            if (_centralIntelligence.GetDrone(_ID))
            {
                //I have absolutely no idea if this is gonna work
                _utility = (float)MathFunction.calculate(_centralIntelligence.GetDrone(_ID).powerLevel);
            }
            else //????
            {
                _utility = 1.0f;
            }
        }
    }

}
