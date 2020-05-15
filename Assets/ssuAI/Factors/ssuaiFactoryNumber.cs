using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using org.mariuszgromada.math.mxparser;
using RTS;


namespace ssuai
{

    /// <summary>
    /// Factor for the number of factories
    /// </summary>
    public class FactoryNumber : Factor
    {
        CentralIntelligence _centralIntelligence;

        public FactoryNumber(CentralIntelligence CI, string function)
        {
            MathFunction = new Function(function);
            _centralIntelligence = CI;
        }

        //TODO unbad this
        public override void UpdateUtility()
        {
            if (GetNoOfFactories()==0)
            {
                _utility = 1;
            } else
            {
                _utility = 0;
            }
        }

        public int GetNoOfFactories()
        {
            return WorldInfo.Factories.Count;
        }
    }
}