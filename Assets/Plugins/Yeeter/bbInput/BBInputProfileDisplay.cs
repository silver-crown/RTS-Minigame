using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yeeter;

/// <summary>
/// Displays BBInputProfile debug info.
/// </summary>
public class BBInputProfileDisplay : MonoBehaviour
{
    [SerializeField] private Text _profileNameText = null;
    [SerializeField] private GameObject _inputAxisDisplay = null;

    public BBInputProfile Profile { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        if (Profile == null)
        {
            InGameDebug.Log(GetType().Name + ": Profile was not set.");
        }
        _profileNameText.text = Profile.Name;
        foreach (var axis in Profile.Axes)
        {
            Instantiate(_inputAxisDisplay, transform).GetComponent<BBInputAxisDisplay>().Axis = axis;
        }
        Canvas.ForceUpdateCanvases();
    }
}
