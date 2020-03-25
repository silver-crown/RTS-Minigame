using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace Progress.InputSystem
{
    public class BuildingSelectionState : MenuState
    {
        // FIXME: Use MenuManager
        public static BuildingSelectionState instance;

        private int playerId = 0;
        private Player player;

        private List<Unit> _units;
        private List<Unit> _selectedUnits;

        private Vector3 _mouseClickPosition;
        private Vector3 _mouseReleasePosition;

        public RectTransform selectionBox;

        public BuildingSelectionState()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Awake()
        {
            MenuManager.Instance.RegisterMenu(this);

            player = ReInput.players.GetPlayer(playerId);
            _units = new List<Unit>();
            _selectedUnits = new List<Unit>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (player.GetButtonDown("Mouse1"))
            {
                _mouseClickPosition = Input.mousePosition;

                if (selectionBox != null)
                {
                    selectionBox.gameObject.SetActive(true);
                }
            }
            if (player.GetButton("Mouse1"))
            {
                Vector3 position1 = _mouseClickPosition;
                Vector3 position2 = Input.mousePosition;
                selectionBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Abs(position2.x - position1.x));
                selectionBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Abs(position2.y - position1.y));
                selectionBox.anchoredPosition3D = (position1 + position2) / 2f;
            }
            if (player.GetButtonUp("Mouse1"))
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
    }
}
