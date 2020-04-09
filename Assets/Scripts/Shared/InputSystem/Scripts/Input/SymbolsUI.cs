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
        private SymbolsContainer _container;

        public enum Direction
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

        public void SetContainer(SymbolsContainer container)
        {
            _container = container;
        }

        public void ChangeUI(Image image)
        {
            _image = image;
        }

        private void LateUpdate()
        {
            Vector3 offset = new Vector3(0f, 0f, 0f);
            Bounds bounds = _container.GetBounds();

            switch (_container.GetDirection(this))
            {
                case Direction.TopLeft:
                    {
                        offset = new Vector3(Mathf.Min(bounds.min.x, bounds.max.x), bounds.max.y, Mathf.Max(bounds.min.z, bounds.max.z));
                        break;
                    }
                case Direction.TopMiddle:
                    {
                        offset = new Vector3(bounds.center.x, bounds.max.y, Mathf.Max(bounds.min.z, bounds.max.z));
                        break;
                    }
                case Direction.TopRight:
                    {
                        offset = new Vector3(Mathf.Max(bounds.min.x, bounds.max.x), bounds.max.y, Mathf.Max(bounds.min.z, bounds.max.z));
                        break;
                    }
                case Direction.Right:
                    {
                        offset = new Vector3(Mathf.Max(bounds.min.x, bounds.max.x), bounds.max.y, bounds.center.z);
                        break;
                    }
                case Direction.BottomRight:
                    {
                        offset = new Vector3(Mathf.Max(bounds.min.x, bounds.max.x), bounds.max.y, Mathf.Min(bounds.min.z, bounds.max.z));
                        break;
                    }
                case Direction.BottomMiddle:
                    {
                        offset = new Vector3(bounds.center.x, bounds.max.y, Mathf.Min(bounds.min.z, bounds.max.z));
                        break;
                    }
                case Direction.BottomLeft:
                    {
                        offset = new Vector3(Mathf.Min(bounds.min.x, bounds.max.x), bounds.max.y, Mathf.Min(bounds.min.z, bounds.max.z));
                        break;
                    }
                case Direction.Left:
                    {
                        offset = new Vector3(Mathf.Min(bounds.min.x, bounds.max.x), bounds.max.y, bounds.center.z);
                        break;
                    }
                default:
                    {
                        throw new System.NotImplementedException();
                    }
            }
            offset -= new Vector3(bounds.center.x, 0f, bounds.center.z);
            transform.position = Camera.main.WorldToScreenPoint(_container.transform.position + offset);
            // + offset.normalized
        }
    }
}
