using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{

    Transform PlayerTransform;

    private Vector3 _cameraOffset;

    //[Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    public bool LookAtPlayer = false;

    public bool RotateAroundPlayer = true;

    float RotationsSpeed = 2f;

    //MusicOrganizer mo;

    //public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    //public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.


    // Use this for initialization
    void Start()
    {
        //mo = FindObjectOfType<MusicOrganizer>();
        //_cameraOffset = new Vector3(0f, 0f, -2000f);
        PlayerTransform = FindObjectOfType<ballSpiral>().transform;
    }


    void LateUpdate()
    {
        _cameraOffset = Camera.main.transform.position;

        //if (!mo.inAudio && !mo.inImage && !mo.inACategory && !mo.inCategories && !mo.firstAnim && !mo.cameraMoving && !mo.isSpiralOpening)
        //{
        Quaternion camTurnAngle =
            Quaternion.AngleAxis(RotationsSpeed, Vector3.up);

        _cameraOffset = camTurnAngle * _cameraOffset;

        Vector3 newPos = Vector3.zero + _cameraOffset;

        Camera.main.transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

        if (LookAtPlayer || RotateAroundPlayer)
            Camera.main.transform.LookAt(PlayerTransform);
        //}
        /*
                if (Input.GetMouseButton(0))
                {
                    Quaternion camTurnAngle =
                        Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationsSpeed, Vector3.up);

                    _cameraOffset = camTurnAngle * _cameraOffset;

                    Vector3 newPos = Vector3.zero + _cameraOffset;

                    transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

                    if (LookAtPlayer || RotateAroundPlayer)
                        transform.LookAt(PlayerTransform);
                }

        */

    }
}
