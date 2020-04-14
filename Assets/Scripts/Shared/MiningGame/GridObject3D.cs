using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Progress.Mining
{
    public class GridObject3D : MonoBehaviour
    {
        private int _height;
        private GridTile3DAdapter _adapter;

        public void Initialize (GridTile3DAdapter adapter, int height)
        {
            _adapter = adapter;
            _height = height;
            transform.position = new Vector3(transform.position.x, transform.position.y + (float)_height, transform.position.z);

            //  Set the color of the material according to the height
            MiningManager manager = GetComponentInParent<MiningManager>();
            Color groundColor = manager.GetColor(_height - 1);
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.color = groundColor;
        }

        public void Remove ()
        {
            _adapter.RemoveCube(this);
            Destroy(this.gameObject);
        }

        public int GetHeight ()
        {
            return _height;
        }
    }
}
