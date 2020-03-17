using UnityEngine;
using UnityEngine.UI;

namespace RTS.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class VariableElementHeightVerticalLayoutGroup : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            float height = 0.0f;
            foreach (RectTransform child in transform)
            {
                child.position = new Vector3(
                    0f,
                    height
                );
                child.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _rectTransform.rect.width);
                height += child.rect.height;
            }
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
    }
}