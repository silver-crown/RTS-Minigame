using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.Mining
{
    public class MiningManager : MonoBehaviour
    {
        [SerializeField]
        private Color[] _colors;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100f))
                {
                    if (hit.collider.CompareTag("Tile"))
                    {
                        GridObject3D cube = hit.collider.gameObject.GetComponent<GridObject3D>();
                        if (cube != null)
                        {
                            cube.Remove();
                        }
                    }
                }
            }
        }

        public Color GetColor (int index)
        {
            if (index > _colors.Length || index < 0)
            {
                Debug.LogError("GetColor: invalid index, returning white");
                return Color.white;
            }
            return _colors[index];
        }
    }
}
