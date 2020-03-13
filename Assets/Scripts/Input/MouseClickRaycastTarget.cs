using System;
using UnityEngine;

public class MouseClickRaycastTarget : MonoBehaviour
{
    public Action OnClick = null;

    public void Click()
    {
        OnClick?.Invoke();
    }
}
