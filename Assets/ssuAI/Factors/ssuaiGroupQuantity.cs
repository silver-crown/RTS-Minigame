using System;
using RTS;
using UnityEngine;
using Bbbt;

namespace ssuai
{
    public class GroupQuantity : Factor
    {
        CentralIntelligence _centralIntelligence;
        private Actor _actor;

        public GroupQuantity(CentralIntelligence CI, string function)
        {
            _centralIntelligence = CI;
        }

        public override void UpdateUtility()
        {
            float groupCount = _actor.GetComponent<CentralIntelligence>().groups.Count;
            float maxGroups = _actor.GetComponent<CentralIntelligence>().maxGroups;
            //a curve that rises quickly in the beginning
            double k = 0.333;

            if (groupCount == 0.0f)
            {
                _utility = 1.0f; // ???
                //i want the utility to get smaller with the closer it gets to maxgroups
            }
            else
            {
                // _utility = (value - min(value)) / (max(value) - min(value));
                //it gets closer to zero the more groups it has
                _utility = 1-(float)Math.Pow(groupCount/maxGroups, k);
            }

            throw new System.NotImplementedException();
        }
    }
}

