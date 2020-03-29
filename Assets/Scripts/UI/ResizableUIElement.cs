using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Yeeter
{
    /// <summary>
    /// Lets a UI element be resized by dragging its edges.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class ResizableUIElement : MonoBehaviour
    {
        [Header("Edge drag areas. You only need to set the edges you want to be act as handles for resizing.")]
        [SerializeField] private RectTransform _top = null;
        [SerializeField] private RectTransform _left = null;
        [SerializeField] private RectTransform _right = null;
        [SerializeField] private RectTransform _bottom = null;
        [Tooltip("If this is enabled, the element will, upon being dragged, be placed topmost of " +
        "elements that are children of the GameObject's parent.")]
        [SerializeField] bool _placeOnTopInParentHierarchy = false;

        private Canvas _canvas;
        private RectTransform _rectTransform;
        private Dictionary<RectTransform, bool> _isBeingResized;

        private Vector3 _previousMousePosition;

        private void Awake()
        {
            _canvas = FindObjectsOfType<Canvas>().Where(
                canvas => canvas.GetComponentsInChildren<ResizableUIElement>().Contains(this)
            ).First();

            if (_canvas == null)
            {
                InGameDebug.Log("<color=red>" + name + ": ResizableUIElement couldn't find its canvas.</color>");
            }

            _rectTransform = GetComponent<RectTransform>();

            _isBeingResized = new Dictionary<RectTransform, bool>();
            if (_top != null) _isBeingResized.Add(_top, false);
            if (_left != null) _isBeingResized.Add(_left, false);
            if (_right != null) _isBeingResized.Add(_right, false);
            if (_bottom != null) _isBeingResized.Add(_bottom, false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Start resizing rects if we're in their area.
                foreach (var rectTransform in _isBeingResized.Keys.ToList())
                {
                    if (IsMouseInRect(rectTransform))
                    {
                        _isBeingResized[rectTransform] = true;
                        if (_placeOnTopInParentHierarchy)
                        {
                            var parent = _rectTransform.parent;
                            _rectTransform.parent = null;
                            _rectTransform.SetParent(parent);
                        }
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {
                var mouseDelta = Input.mousePosition - _previousMousePosition;

                // Resize along any selected axes.
                foreach (var rectTransform in _isBeingResized.Keys)
                {
                    if (_isBeingResized[rectTransform])
                    {
                        if (rectTransform == _top)
                        {
                            _rectTransform.sizeDelta = new Vector2(
                                _rectTransform.sizeDelta.x,
                                _rectTransform.sizeDelta.y + mouseDelta.y
                            );
                            _rectTransform.anchoredPosition = new Vector2(
                                _rectTransform.anchoredPosition.x,
                                _rectTransform.anchoredPosition.y + mouseDelta.y * _rectTransform.pivot.y
                            );
                        }
                        if (rectTransform == _left)
                        {
                            _rectTransform.sizeDelta = new Vector2(
                                _rectTransform.sizeDelta.x - mouseDelta.x,
                                _rectTransform.sizeDelta.y
                            );
                            _rectTransform.anchoredPosition = new Vector2(
                                _rectTransform.anchoredPosition.x + mouseDelta.x * (1 - _rectTransform.pivot.x),
                                _rectTransform.anchoredPosition.y
                            );
                        }
                        if (rectTransform == _bottom)
                        {
                            _rectTransform.sizeDelta = new Vector2(
                                _rectTransform.sizeDelta.x,
                                _rectTransform.sizeDelta.y - mouseDelta.y
                            );
                            _rectTransform.anchoredPosition = new Vector2(
                                _rectTransform.anchoredPosition.x,
                                _rectTransform.anchoredPosition.y + mouseDelta.y * (1 - _rectTransform.pivot.y)
                            );
                        }
                        if (rectTransform == _right)
                        {
                            _rectTransform.sizeDelta = new Vector2(
                                _rectTransform.sizeDelta.x + mouseDelta.x,
                                _rectTransform.sizeDelta.y
                            );
                            _rectTransform.anchoredPosition = new Vector2(
                                _rectTransform.anchoredPosition.x + mouseDelta.x * _rectTransform.pivot.x,
                                _rectTransform.anchoredPosition.y
                            );
                        }
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                // Stop resizing all rects.
                _isBeingResized.Keys.ToList().ForEach(key => _isBeingResized[key] = false);
            }
            _previousMousePosition = Input.mousePosition;
        }

        private bool IsMouseInRect(RectTransform rectTransform)
        {
            bool isMouseInRect = false;
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                isMouseInRect = RectTransformUtility.RectangleContainsScreenPoint(
                    rectTransform, Input.mousePosition, _canvas.worldCamera
                );
            }
            else
            {
                isMouseInRect = RectTransformUtility.RectangleContainsScreenPoint(
                    rectTransform, Input.mousePosition
                );
            }
            return isMouseInRect;
        }
    }
}