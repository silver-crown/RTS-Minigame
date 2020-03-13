using System.Collections.Generic;
using UnityEngine;

namespace RTS.UI.Debugging
{
    public class DroneTableDebugUIElement : MonoBehaviour
    {
        [SerializeField] private GameObject _entryPrefab = null;

        private List<DroneTableDebugUIElementEntry> _entries = null;

        private void Awake()
        {
            if (_entryPrefab == null)
            {
                Debug.LogError(GetType().Name + ": No entry prefab. Set Entry Prefab in the inspector", this);
            }
            else
            {
                Actor.OnActorClicked += (Actor actor) =>
                {
                    if (_entries != null)
                    {
                        foreach (var entry in _entries)
                        {
                            Destroy(entry.gameObject);
                        }
                        _entries = null;
                    }
                    _entries = new List<DroneTableDebugUIElementEntry>();
                    foreach (var pair in actor.GetTablePairs())
                    {
                        AddEntry(pair.Key.ToString().Trim(new char[] { '\"' }), actor);
                    }
                };
            }
        }

        private void AddEntry(string key, Actor actor)
        {
            var entry = Instantiate(_entryPrefab, transform).GetComponent<DroneTableDebugUIElementEntry>();
            entry.Key = key;
            entry.Actor = actor;
            _entries.Add(entry);
        }
    }
}