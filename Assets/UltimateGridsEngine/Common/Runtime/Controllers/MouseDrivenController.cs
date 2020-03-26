using UnityEngine;
using System.Collections;

// Allows you to click anywhere on screen, which will determine a new target and the character will pathfind its way to it
[RequireComponent(typeof(GridPathfinder))]
public class MouseDrivenController : MonoBehaviour {

    protected GridPathfinder _gridPathfinder;
    protected bool _destinationSet = false;

    // On awake we get the GridPathfinder component
    protected virtual void Awake() {
        _gridPathfinder = this.gameObject.GetComponent<GridPathfinder>();
    }

    // On Update we look for a mouse click
    protected virtual void Update() {
        DetectMouse();
    }

    // If the mouse is clicked, we make the currently hovered tile the pathfinding target
    protected virtual void DetectMouse() {
        if (Input.GetMouseButtonDown(0)) {
            if (GridManager.Instance.m_HoveredGridTile != null) {
                _destinationSet = true;
                _gridPathfinder.SetNewDestination(GridManager.Instance.m_HoveredGridTile);
            }
        }
    }
}
