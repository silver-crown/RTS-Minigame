using UnityEngine;

namespace RTS.Test
{
    /// <summary>
    /// A test factory used for spawning drones inn the game world
    /// </summary>
    public class DroneTestFactory : MonoBehaviour
    {
        /// <summary>
        /// prefab for the drone to be spawned
        /// </summary>
        [SerializeField] private GameObject _dronePrefab = null;

        /// <summary>
        /// How long the factory waits before spawning the next drone
        /// </summary>
        [SerializeField] private float _timeBetweenSpawns = 2.0f;

        /// <summary>
        /// The raidus of the area in which the drones can be spawned around the factory
        /// </summary>
        [SerializeField] private float _radius = 20.0f;

        /// <summary>
        /// Enables or disables the possibility for the factory to spawn drones
        /// </summary>
        [SerializeField] private bool _doNotSpawn = false;

        /// <summary>
        /// How long since the factory spawned a drone
        /// </summary>
        private float _timeSinceLastSpawn = 0.0f;

        /// <summary>
        /// Update's the time since drone was spawned, and spawns drones when possible.
        /// </summary>
        private void Update()
        {
            _timeSinceLastSpawn += Time.deltaTime;
            if (_timeSinceLastSpawn >= _timeBetweenSpawns)
            {
                string type = WorldInfo.DroneTypes[Random.Range(0, WorldInfo.DroneTypes.Count)];
                if (!_doNotSpawn) BuildDroneForFree(type);
            }
        }
      
        /// <summary>
        /// Builds a drone.
        /// </summary>
        /// <param name="type">The type of the drone.</param>
        public void BuildDroneForFree(string type)
        {
            var distance = new Vector3(
                Random.Range(-_radius, _radius),
                0.0f,
                Random.Range(-_radius, _radius)
            );
            Drone.Create(type);
        }
    }
}