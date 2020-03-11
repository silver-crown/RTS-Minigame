using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    /// <summary>
    /// A building that actors can enter.
    /// </summary>
    public class EnterableBuilding : MonoBehaviour
    {
        [SerializeField] Transform _entryPoint;    

        private List<Actor> _actorsInBuilding;
        private int _capacity = 10;

        public Vector3 EntryPoint { get; protected set; }

        public bool IsFull
        {
            get => _actorsInBuilding.Count >= _capacity;
        }

        private void Awake()
        {
            if (_entryPoint == null)
            {
                Debug.LogError(
                    GetType().Name + ": No entry point. Set Entry Point in the inspector. " +
                    "Otherwise, actors will not be able to enter.", this);
            }
            else
            {
                EntryPoint = _entryPoint.position;
            }
            _actorsInBuilding = new List<Actor>();
            WorldInfo.EnterableBuildings.Add(this);
        }

        private void Update()
        {
            foreach (var actor in _actorsInBuilding)
            {
                actor.transform.position = transform.position;
            }
        }

        /// <summary>
        /// Adds an actor to the building.
        /// The actor will be confined to the building until Remove() is called on the same actor.
        /// </summary>
        /// <param name="actor">The actor to add.</param>
        public void Add(Actor actor)
        {
            if (!IsFull)
            {
                _actorsInBuilding.Add(actor);
                actor.transform.position = transform.position;
            }
        }

        /// <summary>
        /// Removes an actor from the building.
        /// </summary>
        /// <param name="actor">The actor to remove.</param>
        public void Remove(Actor actor)
        {
            if (_actorsInBuilding.Contains(actor))
            {
                _actorsInBuilding.Remove(actor);
                actor.transform.position = EntryPoint;
            }
            else
            {
                Debug.LogWarning(GetType().Name + ": Tried to remove actor not in building.", this);
            }
        }

        /// <summary>
        /// Checks if the building contains an actor.
        /// </summary>
        /// <param name="actor">The actor to look for.</param>
        /// <returns>True if the actor is in the building, false otherwise.</returns>
        public bool Contains(Actor actor)
        {
            return _actorsInBuilding.Contains(actor);
        }
    }
}