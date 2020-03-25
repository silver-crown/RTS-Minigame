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

            _utility = (float)MathFunction.calculate(groupCount / maxGroups);

        }
    }
}

