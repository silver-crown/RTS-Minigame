using UnityEngine;
using UnityEngine.UI;

namespace RTS.UI.Debugging
{
    public class DronesListDebugUIElementEntry : MonoBehaviour
    {
        [SerializeField] private Text _droneTypeText;
        [SerializeField] private Text _droneCountText;

        private bool error = false;

        public string Type { get; set; }
        public CentralIntelligence CentralIntelligence { get; set; }

        private void Start()
        {
            if (_droneTypeText == null)
            {
                Debug.LogError(GetType().Name + ": _droneTypeText was null. Set Drone Type Text in the inspector.");
                error = true;
            }
            if (_droneCountText == null)
            {
                Debug.LogError(GetType().Name + ": _droneCountText was null. Set Drone Count Text in the inspector.");
                error = true;
            }
            if (CentralIntelligence == null)
            {
                Debug.LogError(GetType().Name + ": CentralIntelligence was null. Set reference after instantiation.");
                error = true;
            }
            if (Type == null)
            {
                Debug.LogError(GetType().Name + ": Type was null. Set drone type after instantiation.");
                error = true;
            }
        }

        private void Update()
        {
            if (!error)
            {
                _droneTypeText.text = Type;
                if (CentralIntelligence.DroneTypeCount.ContainsKey(Type))
                {
                    _droneCountText.text = CentralIntelligence.DroneTypeCount[Type].ToString();
                }
                else
                {
                    _droneCountText.text = "0";
                }
            }
        }
    }
}