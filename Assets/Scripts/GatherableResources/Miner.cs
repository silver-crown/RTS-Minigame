using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

namespace RTS
{

    public class Miner : MonoBehaviour
    {
        public float MiningRange { get; private set; } = 5.0f;
        private float _miningCooldown = 4.0f;
        private int _miningDamage = 5;
        private float _timeLastMined = 0;

        private Drone _worker;
        private Inventory _inventory;

        [SerializeField] public Resource TargetResourceObject = null;


        void Awake()
        {
            _worker = GetComponent<Drone>();
            _inventory = GetComponent<Inventory>();
        }

        /// <summary>
        /// Set mining related values to those found in the given lua file.
        /// </summary>
        /// <param name="luaFile"></param>
        public void Setup(string luaFile)
        {
            Script script = new Script();
            var buildingTable = script.DoFile(luaFile).Table;
            MiningRange = (float)buildingTable.Get("_miningRange").Number;
            _miningCooldown = (float)buildingTable.Get("_miningCooldown").Number;
            _miningDamage = (int)buildingTable.Get("_miningDamage").Number;
        }

        /// <summary>
        /// Mines target, adds resources to inventory.
        /// </summary>
        /// <returns>true if it succeeded in mining anything, false otherwise</returns>
        public bool Mine()
        {
            //if mining cooldonw has finished and we have some space
            if (Time.time >= _timeLastMined && _inventory.GetAvailableSpace() > 0)
            {
                //if we're in range, mine for whatever is smaller of our mining value and our available inventory space and add it to our inventory.
                if (Vector3.Distance(TargetResourceObject.transform.position, transform.position) <= MiningRange)
                {
                    //find out how whether we can mine a full tick's worth
                    int amountToMine = Mathf.Min(_miningDamage, _inventory.GetAvailableSpace());

                    //mine for as much as we can
                    _inventory.Deposit(_worker.TargetResourceType, TargetResourceObject.Mine(amountToMine));

                    //update when our mining cooldown will be done
                    _timeLastMined = Time.time + _miningCooldown;
                    return true; //if that worked out, return true.

                }
            }
            return false;
        }

        /// <summary>
        /// Sets the target resource to parameter game object
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetResourceObject(GameObject target)
        {
            TargetResourceObject = target.GetComponent<Resource>();
            TargetResourceObject.OnDestroyed += OnDestoyed;
        }



        //When our target is destroyed
        void OnDestoyed()
        {
            //set target resource to null
            TargetResourceObject = null;

        }
    }
}