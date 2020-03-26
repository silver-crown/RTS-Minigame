using RTS.Lua;
using System.Collections.Generic;
using UnityEngine;
using Yeeter;

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
                LuaObjectComponent.OnClick += (LuaObjectComponent component) =>
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
                    foreach (var pair in component.GetTablePairs())
                    {
                        AddEntry(pair.Key.ToString().Trim(new char[] { '\"' }), component);
                    }
                };
            }
        }

        private void AddEntry(string key, LuaObjectComponent component)
        {
            var entry = Instantiate(_entryPrefab, transform).GetComponent<DroneTableDebugUIElementEntry>();
            entry.Key = key;
            entry.Component = component;
            _entries.Add(entry);
        }
    }
}