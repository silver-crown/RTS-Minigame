using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.InputSystem
{
    public abstract class BarUI : MonoBehaviour
    {
        private BarController _controller;

        private void LateUpdate()
        {
            Bounds bounds = _controller.GetBounds();
            var overlayManager = _controller.SceneConfig.GetOverlayManager();
            var config = overlayManager.GetConfig(_controller.ConfigName);

            Vector3 offset = new Vector3(bounds.center.x, 0f, Mathf.Max(bounds.min.z, bounds.max.z)) - bounds.center;

            var worldPos = _controller.transform.position + offset + offset.normalized;
            transform.position = Camera.main.WorldToScreenPoint(worldPos);

            // Need to add the offset specified by the overlay manager
            transform.position = new Vector3(transform.position.x, transform.position.y + config.Offset.y, transform.position.z);

            //transform.localScale = new Vector3(bounds.size.x, transform.localScale.y, transform.localScale.z);
        }

        public void SetController(BarController controller)
        {
            _controller = controller;

            OnSetupController(controller);
        }

        public abstract void OnSetupController(BarController controller);
    }
}
