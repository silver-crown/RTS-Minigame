﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bbbt;

namespace ssuai
{

    public class NeedScoutGroup : Factor
    {
        CentralIntelligence _centralIntelligence;
        public NeedScoutGroup(CentralIntelligence CI, string function)
        {
            _centralIntelligence = CI;
        }

        public override void UpdateUtility()
        {
            throw new System.NotImplementedException();
        }
    }
}