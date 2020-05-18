using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bbbt;

namespace ssuai
{
    public class CIBuildFactory : BbbtBehaviour
    {
        public override string SaveDataType => throw new System.NotImplementedException();
        private CentralIntelligence _ci;

        protected override void OnInitialize(GameObject gameObject)
        {
            _ci = GameObject.Find("CI").GetComponent<CentralIntelligence>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {

        }

        /// <summary>
        /// Adds a child to the node.
        /// </summary>
        /// <param name="child">The child to add.</param>
        public override void AddChild(BbbtBehaviour child)
        {

        }

        /// <summary>
        /// Removes all of the behaviour's children.
        /// </summary>
        public override void RemoveChildren()
        {

        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            //Placeholder factory building
            //find a random position not too far from the CI
            //build a factory there

            //generate a random direction
            Vector2 direction = Random.insideUnitCircle.normalized;

            float distance = 12;

            Vector3 factoryLocation = _ci.transform.position + ((Vector3)direction * distance);

            factoryLocation.y = 0;
            
            Instantiate(_ci.FactoryPrefab, factoryLocation, Quaternion.identity);


            return BbbtBehaviourStatus.Success;
        }
    }
}