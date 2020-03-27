using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays a BBInput binding key's name and state in the debug window.
/// </summary>
public class BBInputKeyDisplay : MonoBehaviour
{
    private Text _text;

    public KeyCode Key { get; set; }
    public BBInputAxis Axis { get; set; }

    private void Start()
    {
        _text = GetComponent<Text>();
        _text.text = Key.ToString();
    }

    private void Update()
    {
        _text.color = Color.red;
        if (Input.GetKey(Key))
        {
            _text.color = Color.yellow;
            if (Axis.IsHeld)
            {
                _text.color = Color.green;
            }
        }
    }
}
