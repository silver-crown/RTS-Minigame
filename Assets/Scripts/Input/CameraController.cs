using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _sensitivity = 2.0f;
    [SerializeField] private float _minHeight = 3.6f;

    private Camera _cam;

    private float _yaw;
    private float _pitch;

    void Awake()
    {
        _cam = GetComponent<Camera>();
        _yaw = transform.eulerAngles.y;
        _pitch = transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle movement
        Vector3 movementDirection = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0.0f,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        Vector3 toMove = movementDirection * Time.deltaTime * _speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            toMove *= 2.0f;
        }
        transform.Translate(toMove);
        if (transform.position.y < _minHeight)
        {
            transform.position = new Vector3(
                transform.position.x,
                _minHeight,
                transform.position.z
            );
        }

        // Handle rotation
        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            _yaw += _sensitivity * Input.GetAxis("Mouse X");
            _pitch -= _sensitivity * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);
        }
    }
}