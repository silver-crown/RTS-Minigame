using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.Grid3D
{
    public class GridSquare : MonoBehaviour
    {
        [SerializeField]
        private GridSelectable _building;

        [SerializeField]
        private GridSquarePickable _gridPickable;

        public GridSelectable SelectableGridItem { get { return _building; } }
        public GridSquarePickable PickableContainer { get { return _gridPickable; } }
    }
}
