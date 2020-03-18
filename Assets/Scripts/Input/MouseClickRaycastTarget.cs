using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MouseClickRaycastTarget : MonoBehaviour
{
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

    public void Click()
    {
        OnClick?.Invoke();
    }
}
