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
            _droneTypes = new List<string>();
            var luaFiles = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "Lua", "Actors", "Drones"));
            Debug.Log("Loading drone types...");
            foreach (var file in luaFiles)
            {
                if (file.EndsWith(".lua"))
                {
                    string type = Path.GetFileNameWithoutExtension(file);
                    _droneTypes.Add(Path.GetFileNameWithoutExtension(file));
                    Debug.Log(type);
                }
            }
        }

        private void Update()
        {
            _timeSinceLastSpawn += Time.deltaTime;
            if (_timeSinceLastSpawn >= _timeBetweenSpawns)
            {
                string type = _droneTypes[Random.Range(0, _droneTypes.Count)];
                var distance = new Vector3(
                    Random.Range(-_radius, _radius),
                    0.0f,
                    Random.Range(-_radius, _radius)
                );
                var go = Instantiate(_dronePrefab, transform.position + distance, Quaternion.identity, null);
                Debug.Log("Creating drone of type " + type);
                go.GetComponent<Drone>().SetType(type);
                _timeSinceLastSpawn = 0.0f;
            }
        }
    }
}