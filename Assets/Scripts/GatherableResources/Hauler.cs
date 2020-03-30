using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{

    /// <summary>
    /// Component that lets a worker bring a factory what it needs to build things.
    /// </summary>
    public class Hauler : MonoBehaviour
    {
        private Actor _worker;
        private Inventory _inventory;
        private Inventory _factoryInventory;
        private Queue<GameObject> _depotQueue;

        //The resource type we're hauling
        [SerializeField] public string TargetResourceType { get; private set; } = "Metal";

        void Awake()
        {
            _worker = GetComponent<Actor>();
            _inventory = GetComponent<Inventory>();
        }

        // Update is called once per frame
        void Update()
        {
            //if the factory we're assigned to disappears
            if (_factoryInventory == null)
            {
                // do whatever we do if we lose the thing we're set to. Probably return to base?
            }
        }

        /// <summary>
        /// Sets up the Hauler's queue of depots to visit to withdraw resources
        /// </summary>
        public void InitializeQueue()
        {
            _depotQueue = new Queue<GameObject>();

            List<GameObject> depotList = WorldInfo.Depots;

            depotList.Sort(new CompareListByDistance(gameObject));

            int listLength = depotList.Count;
            for (int i = 0; i<listLength; i++)
            {
                _depotQueue.Enqueue(depotList[i]);
            }
        }

        /// <summary>
        /// Pops a game object from the depot queue
        /// </summary>
        /// <returns></returns>
        public GameObject PopQueue()
        {
            return _depotQueue.Dequeue();
        }

        /// <summary>
        /// Returns whether the queue is empty or not.
        /// </summary>
        /// <returns></returns>
        public bool IsQueueEmpty()
        {
            if (_depotQueue.Count==0)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public void TestAssignFactory()
        {
            //find a factory
            foreach (GameObject structure in WorldInfo.Depots)
            {

                if (structure.GetComponent<Factory>() != null)
                {
                    _factoryInventory = structure.GetComponent<Inventory>();
                    _factoryInventory.GetAvailableSpace(); //test that it was set properly
                    return; //GTFO
                }
            }
        }

        //Comparer for sorting list
        private class CompareListByDistance : Comparer<GameObject>
        {
            private GameObject _go;
            public CompareListByDistance(GameObject go)
            {
                _go = go;
            }
            public override int Compare(GameObject x, GameObject y)
            {
                if (x == null || y == null)
                {
                    return 0;
                }

                float distanceToX = Vector3.Distance(_go.transform.position, x.transform.position);
                float distanceToY = Vector3.Distance(_go.transform.position, y.transform.position);

                if (distanceToX < distanceToY)
                {
                    return -1;
                }
                else if (distanceToX == distanceToY)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }


    }
}