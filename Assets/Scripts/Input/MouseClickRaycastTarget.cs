using System;
using UnityEngine;

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
