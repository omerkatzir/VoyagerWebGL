using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mousemovetest : MonoBehaviour
{

    private float _sensitivity;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private bool _isRotating;

    void Start()
    {
        _sensitivity = 0.25f;
        _rotation = Vector3.zero;
    }

    void Update()
    {
        if (_isRotating)
        {
            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);
            print(_mouseReference);

            // apply rotation
            _rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;

            // rotate
            transform.Rotate(_rotation);

            // store mouse
            _mouseReference = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // rotating flag
            _isRotating = true;

            //// store mouse
            //_mouseReference = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            // rotating flag
            _isRotating = false;
        }
    }

}
