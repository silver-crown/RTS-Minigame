using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays BBInput debug info.
/// </summary>
public class BBInputDebugger : MonoBehaviour
{
    [SerializeField] private GameObject _bbInputProfileDisplay = null;
    private List<BBInputProfile> _registeredProfiles = new List<BBInputProfile>();

    void Update()
    {
        // TODO: Make this event driven.
        foreach (var profile in BBInput.Profiles)
        {
            if (!_registeredProfiles.Contains(profile))
            {
                var display = Instantiate(_bbInputProfileDisplay, transform).GetComponent<BBInputProfileDisplay>();
                display.Profile = profile;
                _registeredProfiles.Add(profile);
                Canvas.ForceUpdateCanvases();
            }
        }
    }
}
