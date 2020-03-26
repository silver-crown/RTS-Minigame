using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.Grid3D
{
    public class GridSquarePickable : MonoBehaviour
    {
        [Header("GridSquare")]
        [SerializeField]
        private GridSquare _gridOwner;

        public GridSquare GridOwner { get { return _gridOwner; } }

        public virtual void Reset()
        {
            _gridOwner = GetComponentInParent<GridSquare>();
            _boxCollider = GetComponent<BoxCollider>();
        }

        [SerializeField]
        private BoxCollider _boxCollider;

        public BoxCollider BoxCollider { get { return _boxCollider; } }

        public virtual void OnMouseOver()
        {
            // FIXME: HARDCODED INPUT
            if (Input.GetMouseButton(0))
            {
                Select();
            }
        }

        public bool TryGetSelectable(out GridSelectable selectable, bool errorIfNull = false)
        {
            var buildingOnGridSquare = GridOwner.SelectableGridItem;

            if (buildingOnGridSquare != null)
            {
                selectable = buildingOnGridSquare;
                return true;
            }
            else
            {
                if (errorIfNull)
                    Debug.LogError("Grid owner does not have selectable grid item to select", this);

                selectable = null;
                return false;
            }
        }

        public bool IsItemSelected
        {
            get
            {
                GridSelectable selectable;
                if(TryGetSelectable(out selectable, true))
                {
                    return selectable.IsSelected;
                }

                return false;
            }
        }

        public void Select()
        {
            GridSelectable selectable;
            if (TryGetSelectable(out selectable, true))
            {
                if(!selectable.IsSelected)
                {
                    Debug.Log("Selected pickable", this);
                    selectable.Select();
                }
            }
        }
    }
}
