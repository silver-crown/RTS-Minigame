using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.Mining
{
    public class GridTile3DAdapter : GridObject
    {
        private List<GridObject3D> _objects = new List<GridObject3D>();

        protected override void Start()
        {
            base.Start();
            _objects = new List<GridObject3D>(GetComponentsInChildren<GridObject3D>());
            if (_objects.Count > 0)
            {
                for (int i = 0; i < _objects.Count; i++)
                {
                    _objects[i].Initialize(this, i + 1);
                }
            }
        }

        public void AddCube (GridObject3D cube, int height)
        {
            if (_objects.Contains(cube))
            {
                Debug.LogError("AddCube: Cube was already in list");
                return;
            }

            if (_objects.Find(x => x.GetHeight() == height))
            {
                Debug.LogError("AddCube: There is already a cube on that height");
                return;
            }

            cube.Initialize(this, height);
            _objects.Add(cube);
        }

        public void RemoveCube (GridObject3D cube)
        {
            if (_objects.Contains(cube))
            {
                _objects.Remove(cube);
            }
            else
            {
                Debug.LogError("RemoveCube: Could not find Cube in list");
            }
        }
    }
}

