using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bbbt;
using System;
using org.mariuszgromada.math.mxparser;


namespace ssuai
{
    /// <summary>
    /// A factor for checking if the CI needs another group of fighters
    /// </summary>
    public class NeedFighterGroup : Factor
    {
        /// <summary>
        /// The Central Intelligence instance the factor will be applied to 
        /// </summary>
        CentralIntelligence _centralIntelligence;

        public NeedFighterGroup(CentralIntelligence CI, string function)
        {
            MathFunction = new Function(function);
            _centralIntelligence = CI;
        }

        public override void UpdateUtility()
        {
            //is he under attack? ?????
            //Are there a lot of fighter groups already?
            //if there's no fighter groups, just set it to 1
            int fighterGroupCount = _centralIntelligence.GetDroneGroupsByType(GroupLeader.GroupType.Assault).Count;
            int groupCount = _centralIntelligence.groups.Count;
            int maxGroups = _centralIntelligence.maxGroups;

            //this is basically the function you put it in, though that was written as "how close are we to being capped on groups" rather than
            //"what percentage of our groups are fighter groups"
            _utility = (float)MathFunction.calculate(groupCount / maxGroups);
        }
    }
}
