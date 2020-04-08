using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace Progress.InputSystem
{
    public class UnitSelectionState : MenuState
    {
        private int playerId = 0;
        private Player player;

        private List<Unit> _units;
        private List<Unit> _selectedUnits;

        private Vector3 _mouseClickPosition;
        private Vector3 _mouseReleasePosition;

        public RectTransform selectionBox;

        [SerializeField]
        private CameraSystem.CameraController _controller;

        private void Awake()
        {
            MenuManager.Instance.RegisterMenu(this);

            player = ReInput.players.GetPlayer(playerId);
            _units = new List<Unit>();
            _selectedUnits = new List<Unit>();
        }

        // Update is called once per frame
        void Update()
        {
            if (player.GetButtonDown(Constants.RewiredInputConstants.mouse1))
            {
                _mouseClickPosition = Input.mousePosition;

                if (selectionBox != null)
                {
                    selectionBox.gameObject.SetActive(true);
                }
            }
            if (player.GetButton(Constants.RewiredInputConstants.mouse1))
            {
                Vector3 position1 = _mouseClickPosition;
                Vector3 position2 = Input.mousePosition;
                selectionBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Abs(position2.x - position1.x));
                selectionBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Abs(position2.y - position1.y));
                selectionBox.anchoredPosition3D = (position1 + position2) / 2f;
            }
            if (player.GetButtonUp(Constants.RewiredInputConstants.mouse1))
            {
                _mouseReleasePosition = Input.mousePosition;
                selectionBox.gameObject.SetActive(false);
                Vector3 min = Vector3.Min(_mouseClickPosition, _mouseReleasePosition);
                Vector3 max = Vector3.Max(_mouseClickPosition, _mouseReleasePosition);
                min.z = Camera.main.nearClipPlane;
                max.z = Camera.main.farClipPlane;
                Bounds selectionBounds = new Bounds();
                selectionBounds.SetMinMax(min, max);
                SelectUnits(selectionBounds);

                //  If the selection box didn't find any units, i.e. if the mouse was just clicked and not dragged
                //  Raycast to see if you clicked a unit
                if (_selectedUnits.Count == 0)
                {
                    RaycastHit hit;
                    Ray ray = _controller.GetComponentInChildren<Camera>().ScreenPointToRay(_mouseReleasePosition);

                    if (Physics.Raycast(ray, out hit, 100f))
                    {
                        if (hit.collider.CompareTag("Unit"))
                        {
                            Unit unitHit = hit.collider.gameObject.GetComponent<Unit>();
                            if (unitHit != null)
                            {
                                unitHit.SetSelected(true);
                                _selectedUnits.Add(unitHit);
                                TransitToUnit(unitHit.gameObject);
                            }
                        }
                    }
                }
            }

            if (player.GetButtonDown(Constants.RewiredInputConstants.mouse2))
            {
                if (_selectedUnits.Count > 0)
                {
                    RaycastHit hit;
                    Ray ray = _controller.GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit, 100f))
                    {
                        if (hit.collider.CompareTag("Tile"))
                        {
                            GridTile tile = hit.collider.gameObject.GetComponent<GridTile>();
                            if (tile != null)
                            {
                                MoveUnitsToTile(tile);
                            }
                        }
                    }
                }
            }
        }

        private void SelectUnits(Bounds bounds)
        {
            foreach (Unit unit in _selectedUnits)
            {
                unit.SetSelected(false);
            }

            _selectedUnits = new List<Unit>();

            foreach (Unit unit in _units)
            {
                if (bounds.Contains(Camera.main.WorldToScreenPoint(unit.transform.position)))
                {
                    _selectedUnits.Add(unit);
                    unit.SetSelected(true);
                }
            }
            if (_selectedUnits.Count > 0)
            {
                TransitToUnit(_selectedUnits[0].gameObject);
            }
        }

        public void AddUnitToList(Unit unit)
        {
            if (!_units.Contains(unit))
            {
                _units.Add(unit);
            }
            else
            {
                Debug.Log("Unit is already in the list, cannot add it again.");
            }
        }

        public void RemoveUnitFromList(Unit unit)
        {
            if (_units.Contains(unit))
            {
                _units.Remove(unit);
            }
            else
            {
                Debug.Log("Unit was not found in the list.");
            }
        }

        private void FollowUnit (GameObject unit)
        {
            CameraSystem.CameraFollow mode = _controller.GetCameraMode<CameraSystem.CameraFollow>();
            if (mode != null)
            {
                mode.Initialise(unit);
                _controller.SetCameraMode<CameraSystem.CameraFollow>();
            }
        }

        private void TransitToUnit (GameObject unit)
        {
            if (_controller != null)
            {
                CameraSystem.CameraTransit mode = _controller.GetCameraMode<CameraSystem.CameraTransit>();
                if (mode != null && _selectedUnits.Count > 0)
                {
                    mode.Initialise(_selectedUnits[0].gameObject, 2f, FollowUnit);
                    _controller.SetCameraMode<CameraSystem.CameraTransit>();
                }
            }
            else
            {
                Debug.LogWarning("Reference to camera controller is not set");
            }
        }

        private void MoveUnitsToTile (GridTile tile)
        {
            foreach (Unit unit in _selectedUnits)
            {
                GridPathfinder pathfinder = unit.GetComponent<GridPathfinder>();
                if (pathfinder != null)
                {
                    //  Movement
                    pathfinder.SetNewDestination(tile);

                    //  Pathfinding visualisation
                    //GridObject gridObject = unit.GetComponent<GridObject>();
                    //if (gridObject != null)
                    //{
                    //    pathfinder.DeterminePath(gridObject.m_CurrentGridTile, tile);
                    //}
                }
                else
                {
                    Debug.Log("No GridMovement component found for this unit");
                }
            }
        }
    }
}