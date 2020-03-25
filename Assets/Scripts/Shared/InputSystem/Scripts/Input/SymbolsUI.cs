using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Progress.InputSystem
{
    public class SymbolsUI : MonoBehaviour
    {
        [SerializeField]
        private Image _image;
        [SerializeField]
        private SymbolsController _symbols;
        [SerializeField]
        private float positionOffset = 0f;
        [SerializeField]
        private float symbolOffsetFromMiddle = 2f;
        [SerializeField]
        private Direction offsetDirection = Direction.Right;
        enum Direction
        {
            TopLeft,
            TopMiddle,
            TopRight,
            Right,
            BottomRight,
            BottomMiddle,
            BottomLeft,
            Left
        }

        public void SetController(SymbolsController symbols)
        {
            _symbols = symbols;
        }

        public void ChangeUI(Image image)
        {
            _image = image;
        }

        private void LateUpdate()
        {
            Vector3 offset = new Vector3(0f, 0f, 0f);
            Bounds bounds = _symbols.GetBounds();

            switch (offsetDirection)
            {
                case Direction.TopLeft:
                    {
                        offset = new Vector3(Mathf.Min(bounds.min.x, bounds.max.x), 0f, Mathf.Max(bounds.min.z, bounds.max.z));
                        break;
                    }
                case Direction.TopMiddle:
                    {
                        offset = new Vector3(bounds.center.x, 0f, Mathf.Max(bounds.min.z, bounds.max.z));
                        break;
                    }
                case Direction.TopRight:
                    {
                        offset = new Vector3(Mathf.Max(bounds.min.x, bounds.max.x), 0f, Mathf.Max(bounds.min.z, bounds.max.z));
                        break;
                    }
                case Direction.Right:
                    {
                        offset = new Vector3(Mathf.Max(bounds.min.x, bounds.max.x), 0f, bounds.center.z);
                        break;
                    }
                case Direction.BottomRight:
                    {
                        offset = new Vector3(Mathf.Max(bounds.min.x, bounds.max.x), 0f, Mathf.Min(bounds.min.z, bounds.max.z));
                        break;
                    }
                case Direction.BottomMiddle:
                    {
                        offset = new Vector3(bounds.center.x, 0f, Mathf.Min(bounds.min.z, bounds.max.z));
                        break;
                    }
                case Direction.BottomLeft:
                    {
                        offset = new Vector3(Mathf.Min(bounds.min.x, bounds.max.x), 0f, Mathf.Min(bounds.min.z, bounds.max.z));
                        break;
                    }
                case Direction.Left:
                    {
                        offset = new Vector3(Mathf.Min(bounds.min.x, bounds.max.x), 0f, bounds.center.z);
                        break;
                    }
                default:
                    {
                        throw new System.NotImplementedException();
                    }
            }
            offset -= bounds.center;
            transform.position = Camera.main.WorldToScreenPoint(_symbols.transform.position + offset + offset.normalized);
        }
    }
}
