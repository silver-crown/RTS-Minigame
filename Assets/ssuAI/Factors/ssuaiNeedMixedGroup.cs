using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ssuai
{
    /// <summary>
    /// A factor for checking if the CI needs another mixed group
    /// </summary>
    public class NeedMixedGroup : Factor
    {
        /// <summary>
        /// The Central Intelligence instance the factor will be applied to 
        /// </summary>
        CentralIntelligence _centralIntelligence;

        public NeedMixedGroup(CentralIntelligence CI, string function) {
            _centralIntelligence = CI;
        }

        public override void UpdateUtility() {
            //is he under attack? ?????
            //Are there a lot of fighter groups already?
            //if there's no fighter groups, just set it to 1
            int fighterGroupCount = _centralIntelligence.GetDroneGroupsByType(Group.GroupType.Mixed).Count;
            int groupCount = _centralIntelligence.groups.Count;
            int maxGroups = _centralIntelligence.maxGroups;
            //do like last time, but take the amount of fighters into account 

            if (fighterGroupCount == 0 && groupCount == 0) {
                _utility = 1.0f;
            }
            else {
                //a curve that rises quickly in the beginning
                double k = 0.333;

                // utility = 1-(value - min(value)) / (max(value) - min(value));
                //it gets closer to zero the more groups it has ^
                _utility = 1 - (float)Math.Pow(groupCount / maxGroups, k);
            }

            throw new System.NotImplementedException();
        }
    }
}
