using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Progress.Grid3D;

namespace Progress.UI
{
    public class SelectableCanvas : BaseCanvas
    {
        [SerializeField]
        private GridSelectable _selectable;

        public void Select(GridSelectable selectable)
        {
            _selectable = selectable;

            Debug.Log("Canvas was spawned for selected item: " + selectable.NiceName, this);
        }
    }
}
