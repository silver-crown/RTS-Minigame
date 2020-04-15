using System;
using UnityEngine;

/// <summary>
/// Acts as a target for MouseClickRaycasters provided that the GameObject it is attached to has a collider.
/// </summary>
public class MouseClickRaycastTarget : MonoBehaviour
{
    /// <summary>
    /// The 
    /// </summary>
    public Action OnClick = null;

    private void Awake()
    {
        var collider = GetComponent<Collider>();
        if (collider == null)
        {
            gameObject.AddComponent<BoxCollider>();
            Debug.LogWarning(GetType().Name + ": No collider on " + name + ". Created BoxCollider.");
        }
    }

    /// <summary>
    /// Invokes the MouseClickRaycastTarget's OnClick.
    /// </summary>
    public void Click()
    {
        OnClick?.Invoke();
    }
}
