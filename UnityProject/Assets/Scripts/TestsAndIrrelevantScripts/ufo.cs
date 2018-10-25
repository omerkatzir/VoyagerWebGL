using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ufo : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 60f, Space.World);
        //transform.position = new Vector3(0f, 0.5f * Mathf.Sin(Time.time * Mathf.PI * 0.1f), 0f);
    }
}
