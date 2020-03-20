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
            double k = 0.333;

            if (groupCount == 0.0f)
            {
                _utility = 1.0f;
            }
            else
            {
                // _utility = (value - min(value)) / (max(value) - min(value));
                //min is 0 so no need to have it here
                 _utility = (float)Math.Pow(groupCount/maxGroups, k);
            }

            throw new System.NotImplementedException();
        }
    }
}

