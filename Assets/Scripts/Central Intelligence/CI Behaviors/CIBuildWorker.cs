using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bbbt;
using MoonSharp.Interpreter;
using RTS;


namespace ssuai
{
    /// <summary>
    /// Central intelligence behavior node for building a worker
    /// </summary>
    public class CIBuildWorker : BbbtBehaviour
    {
        private CentralIntelligence _ci;
        private Table _table;

        public override string SaveDataType => throw new System.NotImplementedException();

        public override void AddChild(BbbtBehaviour child)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveChildren()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInitialize(GameObject gameObject)
        {
            _ci = gameObject.GetComponent<CentralIntelligence>();
            Script script = new Script();
            _table = script.DoFile("Actors.Drones.WorkerDrone").Table;
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
           
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            int metalCost = (int)_table.Get("_metalCost").Number;
            int crystalCost = (int)_table.Get("_crystalCost").Number;

            //if we have enough resources to build a drone
            if (_ci.Inventory.Contents["Metal"]>=metalCost && _ci.Inventory.Contents["Crystal"] >= crystalCost)
            {
                //pay the resource costs of a worker
                _ci.Inventory.Withdraw("Metal", metalCost);
                _ci.Inventory.Withdraw("Crystal", crystalCost);
            } else
            {
                return BbbtBehaviourStatus.Failure;
            }
            
            //find an idle factory, tell it to build a worker

            int lowestQueueLength = 9999;
            Factory selectedFactory = null;

            foreach (GameObject factoryObject in WorldInfo.Factories)
            {
                Factory currentFactory = factoryObject.GetComponent<Factory>();


                if (currentFactory.GetQueueCount() == 0)
                {
                    selectedFactory = currentFactory;
                    //set the selected factory and break
                    break;
                } else if (currentFactory.GetQueueCount() < lowestQueueLength)
                {
                    selectedFactory = currentFactory;
                }
            }

            if (selectedFactory == null)
            {
                Debug.Log("No factories found!");
                return BbbtBehaviourStatus.Failure;
            }

            //tell the factory to build this drone
            //TODO this is kind of a hacky, ask Stig if a better solution exists? 

            selectedFactory.gameObject.GetComponent<ListenToChannel>().ReceiveMessage("Build WorkerDrone");


            return BbbtBehaviourStatus.Success;
        }


    }
}