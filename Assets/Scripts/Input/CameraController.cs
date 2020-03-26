using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _sensitivity = 2.0f;
    [SerializeField] private float _minHeight = 3.6f;

    // Controlled by input
    private float _yaw;
    private float _pitch;
    private bool _doubleSpeed = false;
    private Vector3 _movementDirection = Vector3.zero;

    void Awake()
    {
        _yaw = transform.eulerAngles.y;
        _pitch = transform.eulerAngles.x;

        // Double speed
        BBInput.AddOnAxisPressed("CameraDoubleSpeed", () => { _doubleSpeed = true; });
        BBInput.AddOnAxisReleased("CameraDoubleSpeed",   () => { _doubleSpeed = false; });

        // Movement direction
        BBInput.AddOnAxisHeld("CameraLeft", () => { _movementDirection.x = -1.0f; }, 2);
        BBInput.AddOnAxisHeld("CameraRight", () => { _movementDirection.x =  1.0f; }, 2);
        BBInput.AddOnAxisHeld("CameraForward", () => { _movementDirection.z =  1.0f; }, 2);
        BBInput.AddOnAxisHeld("CameraBack", () => { _movementDirection.z = -1.0f; }, 2);
        BBInput.AddOnAxisReleased("CameraLeft", () => { _movementDirection.x = 0.0f; }, 1);
        BBInput.AddOnAxisReleased("CameraRight", () => { _movementDirection.x = 0.0f; }, 1);
        BBInput.AddOnAxisReleased("CameraForward", () => { _movementDirection.z = 0.0f; }, 1);
        BBInput.AddOnAxisReleased("CameraBack", () => { _movementDirection.z = 0.0f; }, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toMove = _movementDirection.normalized * Time.deltaTime * _speed;
        if (_doubleSpeed)
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