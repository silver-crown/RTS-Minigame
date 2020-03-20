using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bbbt;

namespace ssuai 
{
    /// <summary>
    /// factor for the desire to create worker groups
    /// </summary>
    public class NeedWorkerGroup : Factor
    {
        CentralIntelligence _centralIntelligence;

        public NeedWorkerGroup(CentralIntelligence CI, string function)
        {
            _centralIntelligence = CI;
        }

        public override void UpdateUtility()
        {
            throw new System.NotImplementedException();
        }
    }
}

