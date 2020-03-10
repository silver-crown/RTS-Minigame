using UnityEngine;

namespace RTS.UI.Debugging
{
    public class DronesListDebugUIElement : MonoBehaviour
    {
        [SerializeField] private GameObject _entryPrefab = null;

        private CentralIntelligence _centralIntelligence = null;

        private void Awake()
        {
            // Populate the UI with all the drone types in existence (even if they are not actually used).
        }

        void Start()
        {
            if (_entryPrefab == null)
            {
                Debug.LogError(GetType().Name + ": No list entry prefab. Set Entry Prefab in the inspector", this);
            }
            _centralIntelligence = FindObjectOfType<CentralIntelligence>();
            if (_centralIntelligence == null)
            {
                Debug.LogWarning(GetType().Name + ": No CentralIntelligence in the scene hierarchy.", null);
            }
            else
            {
                foreach (string type in WorldInfo.DroneTypes)
                {
                    AddEntry(type);
                }
            }
        }

        private void AddEntry(string type)
        {
            var entry = Instantiate(_entryPrefab, transform).GetComponent<DronesListDebugUIElementEntry>();
            entry.Type = type;
            entry.CentralIntelligence = _centralIntelligence;
        }
    }
}