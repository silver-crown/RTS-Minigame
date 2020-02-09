using UnityEngine;
using System.Collections;

namespace HGC
{
    public static class CameraExtensions
    {
		public static bool IsPixelOnScreen(this Camera camera, Vector3 pixelCoordinate)
		{
			if (camera == null)
			{
				Debug.LogError("Camera is null");
				return false;
			}

			var screenCoord = camera.ScreenToViewportPoint(pixelCoordinate);

			if (screenCoord.x >= 0 && screenCoord.x <= 1 && screenCoord.y >= 0 && screenCoord.y <= 1)
				return true;
			else
				return false;
		}

        public static bool IsWorldPositionOnScreen(this Camera camera, Vector3 position)
        {
            if (camera == null)
            {
                Debug.LogError("Camera is null");
                return false;
            }

            var screenCoord = camera.WorldToViewportPoint(position);

            if (screenCoord.x >= 0 && screenCoord.x <= 1 && screenCoord.y >= 0 && screenCoord.y <= 1)
                return true;
            else
                return false;
        }

        public static bool IsInFrontOfCamera(this Camera camera, Transform transform)
        {
            if (camera == null)
            {
                Debug.LogError("Camera is null");
                return false;
            }

	        return camera.WorldToScreenPoint(transform.position).z > 0;
        }

        public static bool IsInFrontOfCamera(this Camera camera, Vector3 position)
        {
            if (camera == null)
            {
                Debug.LogError("Camera is null");
                return false;
            }

	        return camera.WorldToScreenPoint(position).z > 0;
        }
    }
}