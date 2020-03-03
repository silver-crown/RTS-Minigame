using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ssuai
{
    
    /// <summary>
    /// Factor for CreateDrone Utility Action - desire to create worker drone goes down the more there are.
    /// </summary>
    public class WorkerNumber : Factor
    {
        CentralIntelligence _centralIntelligence;

        public WorkerNumber(CentralIntelligence CI)
        {
            _centralIntelligence = CI;
        }

        public override float GetUtility()
        {
            Validate();
            return _utility;
        }

        public override void UpdateUtility()
        {
            //Logistic curve that scales on the max number of drones, currently 1/(1+e^(0.02f*(x-MAXDRONES/2)))
            _utility = (float) (1 / (1 + System.Math.Pow(System.Math.E, 0.02f * ( _centralIntelligence.DroneCount - CentralIntelligence.MAXDRONES/2))));
        }
    }
}