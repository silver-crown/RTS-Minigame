using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace Progress.InputSystem
{
    public class CameraControls : MonoBehaviour
    {
        private int playerId = 0;
        private Player player;
        [SerializeField]
        private float cameraSpeed = 3f;
        [SerializeField]
        private float zoomSpeed = 10f;
        private Rigidbody rb;

        private void Start()
        {
            player = ReInput.players.GetPlayer(playerId);
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            //  Movement first, then bounds check. That way we are always inside bounds when rendering.
            float forwardInput = player.GetAxis("Camera Move Forwards/Backwards");
            float sidewaysInput = player.GetAxis("Camera Move Sideways");
            float zoomInput = player.GetAxis("Camera Zoom");

            if (forwardInput != 0f)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * forwardInput * cameraSpeed, Space.World);
            }

            if (sidewaysInput != 0f)
            {
                transform.Translate(Vector3.right * Time.deltaTime * sidewaysInput * cameraSpeed, Space.World);
            }

            if (zoomInput != 0f)
            {
                rb.AddForce(transform.forward * Time.deltaTime * zoomInput * zoomSpeed, ForceMode.Impulse);
            }

            var bounds = GetComponentInParent<SceneConfiguration>().GetBounds();
            float updatedXposition = Mathf.Clamp(transform.position.x, bounds.xNegative, bounds.xPositive);
            float updatedZposition = Mathf.Clamp(transform.position.z, bounds.zNegative, bounds.zPositive);

            if (updatedXposition != transform.position.x || updatedZposition != transform.position.z)
            {
                transform.position = new Vector3(updatedXposition, transform.position.y, updatedZposition);
            }
        }
    }
}
