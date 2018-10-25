using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenshotScript : MonoBehaviour
{

    int num = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            string imageName = num + "_Screenshot.png";
            ScreenCapture.CaptureScreenshot(imageName, 5);
            num++;
        }
    }
}
