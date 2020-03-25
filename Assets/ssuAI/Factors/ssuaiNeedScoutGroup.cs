using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bbbt;
using org.mariuszgromada.math.mxparser;

namespace ssuai
{
    /// <summary>
    /// A factor for checking if the CI needs another scout group
    /// </summary>
    public class NeedScoutGroup : Factor
    {
        CentralIntelligence _centralIntelligence;

        public NeedScoutGroup(CentralIntelligence CI, string function) {
            MathFunction = new Function(function);
            _centralIntelligence = CI;
        }

        public override void UpdateUtility() {
            int groupCount = _centralIntelligence.groups.Count;
            int maxGroups = _centralIntelligence.maxGroups;

            //this is basically the function you put it in, though that was written as "how close are we to being capped on groups" rather than
            //"what percentage of our groups are scout groups"
            _utility = (float)MathFunction.calculate(groupCount / maxGroups);

        }
    }
}