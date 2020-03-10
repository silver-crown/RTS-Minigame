using MoonSharp.Interpreter;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RTS.Test
{
    public class DroneTestFactory : MonoBehaviour
    {
        [SerializeField] private GameObject _dronePrefab = null;
        private List<string> _droneTypes = null;
        private float _radius = 20.0f;
        private float _timeBetweenSpawns = 2.0f;
        private float _timeSinceLastSpawn = 0.0f;

        private void Awake()
        {
            Debug.Log("DroneTestFactory: Setting up...", this);
            // Load drone types
            _droneTypes = new List<string>();
            var luaFiles = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "Lua", "Actors", "Drones"));
            Debug.Log("Loading drone types...", this);
            foreach (var file in luaFiles)
            {
                if (file.EndsWith(".lua"))
                {
                    string type = Path.GetFileNameWithoutExtension(file);
                    _droneTypes.Add(Path.GetFileNameWithoutExtension(file));
                    Debug.Log("\t" + type, this);
                }
            }

            Debug.Log("DroneTestFactory: Setup complete!", this);
        }

        private void Update()
        {
            _timeSinceLastSpawn += Time.deltaTime;
            if (_timeSinceLastSpawn >= _timeBetweenSpawns)
            {
                string type = _droneTypes[Random.Range(0, _droneTypes.Count)];
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
            Debug.Log("Creating drone of type " + type);
            drone.SetType(type);
            _timeSinceLastSpawn = 0.0f;
            GetComponent<CentralIntelligence>().AddDrone(drone);
        }
    }
}