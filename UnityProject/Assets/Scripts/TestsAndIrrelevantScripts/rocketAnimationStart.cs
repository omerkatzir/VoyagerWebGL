using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketAnimationStart : MonoBehaviour
{
    float t;
    float duration = 5f;

    // Use this for initialization
    void Start()
    {
        t = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        var timer = (Time.time - t) / duration;
        float yyy = Mathf.SmoothStep(2f, -23f, timer);
        transform.position = new Vector3(0f, yyy, 0f);

        if (timer >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
