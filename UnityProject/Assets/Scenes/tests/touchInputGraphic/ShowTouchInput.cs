using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTouchInput : MonoBehaviour
{
    public List<GameObject> touchesGraphic = new List<GameObject>();
    RectTransform rt;


    // Use this for initialization
    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {


        if (Input.touchCount > 0)
        {
            TouchManager();
        }

        //if (Input.touchCount == 0)
        //{
        //    foreach (var g in touchesGraphic)
        //    {
        //        g.SetActive(false);
        //    }
        //}
    }

    //void ChangeSize(Touch curTouch){
    //    if (touchStarted)
    //    {
    //        var size = Mathf.Lerp(7.5f, 6f, timer);

    //    }

    //    if (touchEnded)
    //    {
    //        var size = Mathf.Lerp(6f, 7.5f, timer);
    //    }
    //}

    void TouchManager()
    {

        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                touchesGraphic[i].SetActive(true);
                var csac = touchesGraphic[i].GetComponent<ChangeSizeAndColor>();
                csac.startTime = Time.time;
                csac.timerRunning = true;
                csac.touchStarted = true;

            }

            if (Input.GetTouch(i).phase == TouchPhase.Ended || Input.GetTouch(i).phase == TouchPhase.Canceled)
            {
                //if(Input.to)
                var csac = touchesGraphic[i].GetComponent<ChangeSizeAndColor>();
                csac.startTime = Time.time;
                csac.timerRunning = true;
                csac.touchEnded = true;

                //touchesGraphic[i].SetActive(false);
            }

            if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Moved)
            {
                Vector2 pos = Input.GetTouch(i).position;
                touchesGraphic[i].transform.position = pos;
                //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.GetTouch(i).position, Camera.main, out pos))
                //{

                //    touchesGraphic[i].transform.position = rt.transform.TransformPoint(pos);
                //}

            }

        }
    }
}
