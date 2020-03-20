using UnityEngine;
using UnityEngine.UI;

namespace RTS.UI.Debugging
{
    public class ResourceListDebugUIElementEntry : MonoBehaviour
    {
        [SerializeField] private Text _resourceTypeText;
        [SerializeField] private Text _resourceCountText;

        private bool error = false;

        public string Type { get; set; }
        public CentralIntelligence CentralIntelligence { get; set; }

        private void Start()
        {
            if (_resourceTypeText == null)
            {
                Debug.LogError(
                    GetType().Name + ": _resourceTypeText was null. Set Resource Type Text in the inspector.");
                error = true;
            }
            if (_resourceCountText == null)
            {
                Debug.LogError(
                    GetType().Name + ": _resourceCountText was null. Set Resource Count Text in the inspector.");
                error = true;
            }
            if (CentralIntelligence == null)
            {
                Debug.LogError(GetType().Name + ": CentralIntelligence was null. Set reference after instantiation.");
                error = true;
            }
            if (Type == null)
            {
                Debug.LogError(GetType().Name + ": Type was null. Set resource type after instantiation.");
                error = true;
            }
        }

        private void Update()
        {
            if (!error)
            {
                _resourceTypeText.text = Type;
                if (CentralIntelligence.Inventory.Contents.ContainsKey(Type))
                {
                    _resourceCountText.text = CentralIntelligence.Inventory.Contents[Type].ToString();
                }
                else
                {
                    _resourceCountText.text = "0";
                }
            }
        }
    }
}