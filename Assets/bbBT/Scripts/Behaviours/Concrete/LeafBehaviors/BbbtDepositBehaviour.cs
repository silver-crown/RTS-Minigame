﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Bbbt
{

    [CreateAssetMenu(
        fileName = "Deposit",
        menuName = "bbBT/Behaviour/Leaf/Deposit",
        order = 0)]
    public class BbbtDepositBehaviour : BbbtLeafBehaviour
    {
        private Drone _drone;
        private Inventory _inventory;
        private Inventory _depot;

        public override string SaveDataType { get; } = "BbbtDepositBehaviour";

        protected override void OnInitialize(GameObject gameObject)
        {
            _inventory = gameObject.GetComponent<Inventory>();
            _drone = gameObject.GetComponent<Drone>();
            _depot = _drone.CentralIntelligence.GetComponent<Inventory>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
        }

        /// <summary>
        /// Deposit resources to the central storage at the CI
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            //Debug.Log("Henlo");
            Debug.Log(_drone.TargetResourceType);
            string typeToDeposit = _drone.TargetResourceType;
            //Withdraw all items in our inventory of our current target resource type, and put them in the depot's inventory.

            List<string> typeList = _inventory.GetKeyList();
            foreach(string type in typeList)
            {
                int amountToDeposit = _inventory.Withdraw(type, _inventory.Contents[typeToDeposit]);
                int remainder = amountToDeposit - _depot.Deposit(typeToDeposit, amountToDeposit);

                //if we couldn't deposit all of it, return failure
                if (remainder != 0)
                {
                    return BbbtBehaviourStatus.Failure;
                }
            }
            //if there's nothing left over, we succeeded.
                return BbbtBehaviourStatus.Success;               
        }
    }
}