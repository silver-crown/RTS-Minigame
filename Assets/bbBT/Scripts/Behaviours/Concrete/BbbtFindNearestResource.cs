using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Bbbt
{
    [CreateAssetMenu(
        fileName = "FindNearestResource",
        menuName = "bbBT/Behaviour/Leaf/FindNearestResource",
        order = 0)]
    public class BbbtFindNearestResource : BbbtLeafBehaviour
    {
        private Miner _miner;

        public override string SaveDataType { get; } = "FindNearestResource";

        protected override void OnInitialize(GameObject go)
        {
            _miner = go.GetComponent<Miner>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {

        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            GameObject currentTarget = null;
            float distanceToCurrentTarget = Mathf.Infinity;

            foreach (GameObject resource in WorldInfo.Resources)
            {

                //if the resource is the type we're looking for
                if (resource.GetComponent<Resource>().ResourceType == _miner.TargetResourceType)
                {
                    //if the currently processed resource is closer than our current target
                    float distanceToProcessing = Vector3.Distance(_miner.transform.position, resource.transform.position);

                    if (distanceToProcessing < distanceToCurrentTarget)
                    {
                        //set the processed object as the new target
                        currentTarget = resource.gameObject;
                        distanceToCurrentTarget = distanceToProcessing;
                    }
                }
            }

            //if we found a resource to target, we did it, woo
            if (currentTarget != null)
            {
                _miner.SetTargetResource(currentTarget);
                return BbbtBehaviourStatus.Success;
            } else
            {
                //otherwise we're a failure
                return BbbtBehaviourStatus.Failure;
            }
        }
    }
}