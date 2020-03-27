using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yeeter;

/// <summary>
/// Displays one of a BBInputAxis's bindings' debug info.
/// </summary>
public class BBInputBindingDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _bindingKeyEntryPrefab = null;

    public List<KeyCode> Binding { get; set; }
    public BBInputAxis Axis { get; set; }

    private void Start()
    {
        foreach (var key in Binding)
        {
            var keyDisplay = Instantiate(_bindingKeyEntryPrefab, transform).GetComponent<BBInputKeyDisplay>();
            keyDisplay.Key = key;
            keyDisplay.Axis = Axis;
        }
        Canvas.ForceUpdateCanvases();
    }
}
