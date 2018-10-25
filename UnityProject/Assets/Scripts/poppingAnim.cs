using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poppingAnim : MonoBehaviour
{

    public float amplitude = 7f;
    float duration = 0.5f;
    float startTime;
    public Vector3 orgSize;

    bool isRunning = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            float t = (Time.time - startTime) / duration;

            moveInCos(t);
            //c = new Color(1, 1, 1, t);
            //r.material.color = c;

            if (t >= 1)
            {
                isRunning = false;
            }

        }


    }

    void moveInCos(float t)
    {

        var tempSize = Vector3.Lerp(Vector3.zero, orgSize, t);
        //tempPos = transform.position;
        transform.localScale = tempSize;
        //tempPos.y = amplitude * Mathf.Cos(Mathf.SmoothStep(0f, Mathf.PI, t)) + amplitude;
        //transform.position = tempPos;
    }

    public void startTimer()
    {
        startTime = Time.time;
        isRunning = true;
    }
}
