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
        BBInput.AddOnKeyDown(KeyCode.LeftShift, () => { _doubleSpeed = true; });
        BBInput.AddOnKeyUp(KeyCode.LeftShift,   () => { _doubleSpeed = false; });

        // Movement direction
        BBInput.AddOnKey(KeyCode.A, () => { _movementDirection.x = -1.0f; }, 2);
        BBInput.AddOnKey(KeyCode.D, () => { _movementDirection.x =  1.0f; }, 2);
        BBInput.AddOnKey(KeyCode.W, () => { _movementDirection.z =  1.0f; }, 2);
        BBInput.AddOnKey(KeyCode.S, () => { _movementDirection.z = -1.0f; }, 2);
        BBInput.AddOnKeyUp(KeyCode.A, () => { _movementDirection.x = 0.0f; }, 1);
        BBInput.AddOnKeyUp(KeyCode.D, () => { _movementDirection.x = 0.0f; }, 1);
        BBInput.AddOnKeyUp(KeyCode.W, () => { _movementDirection.z = 0.0f; }, 1);
        BBInput.AddOnKeyUp(KeyCode.S, () => { _movementDirection.z = 0.0f; }, 1);
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