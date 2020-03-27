﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using org.mariuszgromada.math.mxparser;

namespace ssuai
{
    /// <summary>
    /// A factor for checking if the CI needs another defensive group
    /// </summary>
    public class NeedDefenseGroup : Factor
    {
        /// <summary>
        /// The instance of central intelligence the factor will be applied to
        /// </summary>
        CentralIntelligence _centralIntelligence;

        public NeedDefenseGroup(CentralIntelligence CI, string function) {
            MathFunction = new Function(function);
            _centralIntelligence = CI;
        }

        public override void UpdateUtility() {
            //is he under attack? ?????
            //Are there a lot of fighter groups already?
            //if there's no fighter groups, just set it to 1
            int fighterGroupCount = _centralIntelligence.GetDroneGroupsByType(GroupLeader.GroupType.Defense).Count;
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
