using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Progress.UI;
using Progress.InputSystem;

namespace Progress.Grid3D
{
    public abstract class GridSelectable : Selectable
    {
        [Header("GridSquare")]
        [SerializeField]
        private GridSquare _gridOwner;

        public GridSquare GridOwner { get { return _gridOwner; } }

        public virtual void Reset()
        {
            _gridOwner = GetComponentInParent<GridSquare>();
        }

        [Header("Selectable")]
        [SerializeField]
        private GameObject _selectableCanvasPrefab;

        [SerializeField]
        private GameObject _selectableCanvasInstance;

        [SerializeField]
        private SelectableCanvas _selectableCanvas;

        [SerializeField]
        private bool _isSelected;

        public virtual string NiceName
        {
            get
            {
                return gameObject.name;
            }
        }

        public bool IsSelected { get { return _isSelected; } }

        public virtual void Select()
        {
            // TODO: what if Canvas already exists
            InstantiateWorldSpaceCanvas();
        }

        private void InstantiateWorldSpaceCanvas()
        {
            if (_selectableCanvasPrefab == null)
            {
                Debug.LogError("Cannot open canvas, since prefab ref is null", this);
                return;
            }
        
            Debug.Log("Instantiating Worldspace Canvas " + gameObject.name, this);

            _selectableCanvasInstance = Instantiate(_selectableCanvasPrefab, transform);

            _selectableCanvas = _selectableCanvasInstance.GetComponent<SelectableCanvas>();
            _selectableCanvas.Select(this);
            _isSelected = true;
        }
    }
}
