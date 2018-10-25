using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    bool swipeUp, swipeDown, swipeLeft, swipeRight, isDragging;
    Vector2 startTouch, swipeDelta;

    float previousDistance;
    float zoomSpeed = 0.1f;

    MusicOrganizer mo;

    private void Reset()
    {
        isDragging = false;
        startTouch = Vector2.zero;
        swipeDelta = Vector2.zero;
    }

    private void Start()
    {
        mo = FindObjectOfType<MusicOrganizer>();

    }

    // Update is called once per frame
    void Update()
    {
        swipeUp = swipeDown = swipeLeft = swipeRight = false;

        if (Input.GetMouseButton(0))
        {
            swipeControlMouse();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            Reset();
        }


        if (Input.touchCount > 0) // && !mo.cameraMoving && !mo.firstAnim && !mo.isSpiralOpening)
        {

            if (Input.touchCount == 1)
            {
                swipeControlTouch();
            }
            if (Input.touchCount == 2)
            {
                pinchControl();
            }
        }

        if (swipeUp)
        {
            print("swipeUP");
        }
        if (swipeDown)
        {
            print("swipeDown");
        }
        if (swipeRight)
        {
            print("swipeRight");
        }
        if (swipeLeft)
        {
            print("swipeLeft");
        }
    }

    // top max should be 2000 y, and bottom -2000 y

    void swipeControlMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startTouch = (Vector2)Input.mousePosition;//Input.GetTouch(0).position;
        }

        if (isDragging)
        {
            swipeDelta = ((Vector2)Input.mousePosition) - startTouch;
        }

        if (swipeDelta.magnitude > 125f)
        {

            //which direction?
            var x = swipeDelta.x;
            var y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //left or right?
                if (x < 0f)
                {
                    swipeLeft = true;
                }
                if (x > 0f)
                {
                    swipeRight = true;
                }
            }
            if ((Mathf.Abs(x) < Mathf.Abs(y)))
            {
                // up or down
                if (y < 0f)
                {
                    swipeDown = true;
                }
                if (y > 0f)
                {
                    swipeUp = true;
                }
            }

            Reset();
        }
    }

    void swipeControlTouch()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            isDragging = true;
            startTouch = Input.GetTouch(0).position;
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
        {
            Reset();
        }

        if (isDragging)
        {
            swipeDelta = Input.GetTouch(0).position - startTouch;
        }

        //did we cross the swipe deadzone?
        if (swipeDelta.magnitude > 125f)
        {

            //which direction?
            var x = swipeDelta.x;
            var y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //left or right?
                if (x < 0f)
                {
                    swipeLeft = true;
                }
                if (x > 0f)
                {
                    swipeRight = true;
                }
            }
            if ((Mathf.Abs(x) < Mathf.Abs(y)))
            {
                // up or down
                if (y < 0f)
                {
                    swipeDown = true;
                }
                if (y > 0f)
                {
                    swipeUp = true;
                }
            }

            Reset();
        }
    }

    void pinchControl()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
        {
            //caliberater distance
            previousDistance = (Input.GetTouch(0).position - Input.GetTouch(1).position).sqrMagnitude;
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
        {

            Vector2 touch1 = Input.GetTouch(0).position;
            Vector2 touch2 = Input.GetTouch(1).position;

            float distance = (touch1 - touch2).sqrMagnitude;

            //camera based on pinch/zoom
            float pinchAmount = ((previousDistance - distance) * -1f) * zoomSpeed * Time.deltaTime;

            Camera.main.transform.Translate(0f, 0f, pinchAmount);

            ////limits the camera movement
            //if (Camera.main.transform.position.z < -5000f)
            //{
            //    Camera.main.transform.position = new Vector3(0f, 0f, -5000f);
            //}
            //if (Camera.main.transform.position.z > -1500f)
            //{
            //    Camera.main.transform.position = new Vector3(0f, 0f, -1500f);
            //}

            previousDistance = distance;
        }
    }
}
