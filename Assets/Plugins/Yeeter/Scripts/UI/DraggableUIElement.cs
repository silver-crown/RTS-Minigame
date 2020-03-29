using System.Linq;
using UnityEngine;

namespace Yeeter
{
    /// <summary>
    /// Makes it possible to drag a UI element around.
    /// </summary>
    public class DraggableUIElement : MonoBehaviour
    {
        [Tooltip("The handle's RectTransform." +
        "If the player presses inside the handle they'll be able to drag the UI element.")]
        [SerializeField] RectTransform _handle = null;
        [Tooltip("If true the entire element must be inside its parent.")]
        [SerializeField] bool _constrainToParentBounds = false;

        Canvas _canvas;
        RectTransform _rectTransform;
        RectTransform _parentRectTransform;

        bool _isDragging = false;
        Vector3 _previousMousePosition;

        private void Awake()
        {
            _canvas = FindObjectsOfType<Canvas>().Where(
                canvas => canvas.GetComponentsInChildren<DraggableUIElement>().Contains(this)
            ).First();

            if (_canvas == null)
            {
                InGameDebug.Log("<color=red>" + name + ": ResizableUIElement couldn't find its canvas.</color>");
            }

            _rectTransform = GetComponent<RectTransform>();
            _parentRectTransform = _rectTransform.parent.GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Start dragging.
                if (IsMouseInRect())
                {
                    _isDragging = true;
                }
            }
            if (_isDragging && Input.GetMouseButton(0))
            {
                // Drag.
                var mouseDelta = Input.mousePosition - _previousMousePosition;
                _rectTransform.anchoredPosition += new Vector2(mouseDelta.x, mouseDelta.y);

                if (_constrainToParentBounds)
                {
                    // Limit the rect transform to be inside its parent.
                    // 0 = bottom left, 1 = bottom right, 2 = top right, 3 = top left
                    var corners = new Vector3[4];
                    _rectTransform.GetWorldCorners(corners);
                    var parentCorners = new Vector3[4];
                    _parentRectTransform.GetWorldCorners(parentCorners);
                    var min = corners[0] - parentCorners[0];
                    var max = corners[2] - parentCorners[2];
                    // Left.
                    if (mouseDelta.x < 0.0f && min.x < 0.0f)
                    {
                        _rectTransform.position -= Vector3.right * min.x;
                    }
                    // Right
                    if (mouseDelta.x > 0.0f && max.x > 0.0f)
                    {
                        _rectTransform.position -= Vector3.right * max.x;
                    }
                    // Top
                    if (mouseDelta.y > 0.0f && max.y > 0.0f)
                    {
                        _rectTransform.position -= Vector3.up * max.y;
                    }
                    // Bottom
                    if (mouseDelta.y < 0.0f && min.y < 0.0f)
                    {
                        _rectTransform.position -= Vector3.up * min.y;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                // Stop dragging.
                _isDragging = false;
            }
            _previousMousePosition = Input.mousePosition;
        }

        private bool IsMouseInRect()
        {
            bool isMouseInRect = false;
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                isMouseInRect = RectTransformUtility.RectangleContainsScreenPoint(
                    _handle, Input.mousePosition, _canvas.worldCamera
                );
            }
            else
            {
                isMouseInRect = RectTransformUtility.RectangleContainsScreenPoint(
                    _handle, Input.mousePosition
                );
            }
            return isMouseInRect;
        }
    }
}