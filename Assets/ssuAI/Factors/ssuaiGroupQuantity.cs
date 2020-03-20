using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bbbt;

namespace ssuai
{
    public class GroupQuantity : Factor
    {
        CentralIntelligence _centralIntelligence;

        public GroupQuantity(CentralIntelligence CI, string function)
        {
            _centralIntelligence = CI;
        }

        public override void UpdateUtility()
        {
            throw new System.NotImplementedException();
        }
    }
}

