using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Bbbt
{
    [CreateAssetMenu(
        fileName = "FindNearestDepot",
        menuName = "bbBT/Behaviour/Leaf/Find Nearest Depot",
        order = 0)]
    public class BbbtFindNearestDepot : BbbtLeafBehaviour
    {
        private Drone _drone;

        public override string SaveDataType { get; } = "BbbtFindNearestDepot";

        protected override void OnInitialize(GameObject go)
        {
            _drone = go.GetComponent<Drone>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {

        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            GameObject currentTarget = null;
            float distanceToCurrentTarget = Mathf.Infinity;


            foreach (GameObject depot in WorldInfo.Depots)
            {
                //if the depot has space
                if (depot.GetComponent<Inventory>().GetAvailableSpace() > 0)
                {
                    //if the currently processed depot is closer than our current target
                    float distanceToProcessing = Vector3.Distance(_drone.transform.position, depot.transform.position);

                    if (distanceToProcessing < distanceToCurrentTarget)
                    {
                        //set the processed object as the new target
                        currentTarget = depot.gameObject;
                        distanceToCurrentTarget = distanceToProcessing;
                    }
                }
            }

            //if we found a depot to target, we did it, woo
            if (currentTarget != null)
            {
                _drone.SetTargetDepot(currentTarget);
                return BbbtBehaviourStatus.Success;
            }
            else
            {
                //otherwise we're a failure
                Debug.Log("Could not find depot!");
                return BbbtBehaviourStatus.Failure;
            }
        }
    }
}