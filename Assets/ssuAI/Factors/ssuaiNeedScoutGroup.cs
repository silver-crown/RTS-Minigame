using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bbbt;
using org.mariuszgromada.math.mxparser;

namespace ssuai
{
    public class NeedScoutGroup : Factor
    {
        CentralIntelligence _centralIntelligence;
        public NeedScoutGroup(CentralIntelligence CI, string function)
        {
            MathFunction = new Function(function);
            _centralIntelligence = CI;
        }

        public override void UpdateUtility()
        {
            //TODO Fix this
            _utility = 0.0f;
        }
    }
}