using MoonSharp.Interpreter;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RTS.Test
{
    public class DroneTestFactory : MonoBehaviour
    {
        [SerializeField] private GameObject _dronePrefab = null;
        private float _radius = 20.0f;
        private float _timeBetweenSpawns = 2.0f;
        private float _timeSinceLastSpawn = 0.0f;

        private void Update()
        {
            _timeSinceLastSpawn += Time.deltaTime;
            if (_timeSinceLastSpawn >= _timeBetweenSpawns)
            {
                string type = WorldInfo.DroneTypes[Random.Range(0, WorldInfo.DroneTypes.Count)];
                BuildDroneForFree(type);
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
            var go = Instantiate(_dronePrefab, transform.position + distance, Quaternion.identity, null);
            var drone = go.GetComponent<Drone>();
            drone.SetType(type);
            _timeSinceLastSpawn = 0.0f;
            GetComponent<CentralIntelligence>().AddDrone(drone);
        }
    }
}