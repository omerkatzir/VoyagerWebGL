using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectMover : MonoBehaviour
{

    public bool isMoving = false;
    public bool isRotating = false;
    public float startTime;
    public float duration;
    public Vector3 newPos;
    public Vector3 oldPos;

    public Vector3 newEul;
    public Vector3 oldEul;

    public void GetOldPos()
    {
        oldPos = gameObject.transform.position;
    }

    public void GetOldEul()
    {
        oldEul = gameObject.transform.rotation.eulerAngles;
    }

    public void MoveToNewPlace()
    {
        float t = (Time.time - startTime) / duration;
        Vector3 move = new Vector3(Mathf.SmoothStep(oldPos.x, newPos.x, t), Mathf.SmoothStep(oldPos.y, newPos.y, t), Mathf.SmoothStep(oldPos.z, newPos.z, t));
        transform.position = move;

        if (t >= 1f)
        {
            isMoving = false;
        }
    }

    public void RotateEul()
    {
        float t = (Time.time - startTime) / duration;
        Quaternion currentEul = Quaternion.Euler(oldEul);
        Quaternion rotEul = Quaternion.Slerp(currentEul, Quaternion.Euler(newEul), t);
        transform.rotation = rotEul;
    }

    private void Start()
    {
        duration = Random.Range(3f, 7f);
    }

    void Update()
    {
        if (isMoving)
        {
            MoveToNewPlace();
        }
        if (isRotating)
        {
            RotateEul();
        }
    }
}
