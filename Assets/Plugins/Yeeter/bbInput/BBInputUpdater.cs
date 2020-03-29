using UnityEngine;
using Yeeter;

/// <summary>
/// Updates BBInput profiles and axes and triggers events.
/// </summary>
public class BBInputUpdater : MonoBehaviour
{
    private void Update()
    {
        // Update all input axis states.
        foreach (var profile in BBInput.Profiles)
        {
            // Skip disabled profiles.
            if (!profile.Enabled) continue;
            foreach (var axis in profile.Axes)
            {
                bool wasHeld = axis.IsHeld;
                axis.IsHeld = false;
                axis.WasPressedThisFrame = false;
                axis.WasReleasedThisFrame = false;
                foreach (var binding in axis.PositiveKeyCodes)
                {
                    int pressed = 0;
                    int held = 0;
                    foreach (var key in binding)
                    {
                        if (Input.GetKeyDown(key)) pressed++;
                        if (Input.GetKey(key)) held++;
                    }
                    if (held == binding.Count)
                    {
                        if (pressed > 0) axis.WasPressedThisFrame = true;
                        axis.IsHeld = true;
                    }
                }
                if (wasHeld && !axis.IsHeld)
                {
                    axis.WasReleasedThisFrame = true;
                }
            }
        }
    }
}