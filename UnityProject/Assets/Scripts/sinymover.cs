using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sinymover : MonoBehaviour
{

    public float amplitude = 7f;
    float duration = 0.5f;
    float startTime;
    public Vector3 tempPos;
    Color c;
    public int cubenumber;
    public GameObject camTransOBJ;
    public Transform camTrans;

    //Renderer r;

    // Use this for initialization
    void Start()
    {
        camTransOBJ = new GameObject("camTransOBJ");
        camTrans = camTransOBJ.transform;
        camTransOBJ.transform.parent = gameObject.transform;

        tempPos = transform.position;
        startTime = Time.time;
        //r = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

        float t = (Time.time - startTime) / duration;
        if (t <= 1)
        {
            moveInCos(t);
            //c = new Color(1, 1, 1, t);
            //r.material.color = c;
        }


    }

    void moveInCos(float t)
    {

        var tempSize = new Vector3(Mathf.SmoothStep(0f, 0.6f, t), Mathf.SmoothStep(0f, 0.6f, t), Mathf.SmoothStep(0f, 0.6f, t));
        //tempPos = transform.position;
        transform.localScale = tempSize;
        //tempPos.y = amplitude * Mathf.Cos(Mathf.SmoothStep(0f, Mathf.PI, t)) + amplitude;
        //transform.position = tempPos;
    }
}
