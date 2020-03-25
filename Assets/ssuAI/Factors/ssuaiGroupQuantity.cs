using System;
using RTS;
using UnityEngine;
using Bbbt;
using org.mariuszgromada.math.mxparser;

namespace ssuai
{
    public class GroupQuantity : Factor
    {
        CentralIntelligence _centralIntelligence;
        private Actor _actor;

        public GroupQuantity(CentralIntelligence CI, string function)
        {
            MathFunction = new Function(function);
            _centralIntelligence = CI;
        }

        public override void UpdateUtility()
        {
            int groupCount = _centralIntelligence.groups.Count;
            int maxGroups = _centralIntelligence.maxGroups;
            //a curve that rises quickly in the beginning
            double k = 0.333;

            // utility = 1-(value - min(value)) / (max(value) - min(value));
             //it gets closer to zero the more groups it has ^
            _utility = 1-(float)Math.Pow(groupCount/maxGroups, k);

        }
    }
}

