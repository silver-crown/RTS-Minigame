using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yeeter;

/// <summary>
/// Displays BBInputAxis debug info.
/// </summary>
public class BBInputAxisDisplay : MonoBehaviour
{
    [SerializeField] private Text _axisNameText = null;
    [SerializeField] private GameObject _bindingsDisplayPrefab = null;

    public BBInputAxis Axis { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        if (Axis == null)
        {
            InGameDebug.Log(GetType().Name + ": Axis was not set.");
        }
        _axisNameText.text = Axis.Name;
        foreach (var binding in Axis.PositiveKeyCodes)
        {
            var bindingDisplay = Instantiate(_bindingsDisplayPrefab, transform).GetComponent<BBInputBindingDisplay>();
            bindingDisplay.Binding = binding;
            bindingDisplay.Axis = Axis;
        }
        Canvas.ForceUpdateCanvases();
    }
}
