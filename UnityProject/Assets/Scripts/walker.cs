using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walker : MonoBehaviour
{

    //public spiralSpline spline;
    //public spiralCurve spiral;
    public BezierSpline spline;

    public float duration;

    public float progress;

    public float m_Speed = 20f;

    //private Vector3 m_RotationDirection = Vector3.up;

    private void Update()
    {
        progress += Time.deltaTime / duration;
        if (progress > 1f)
        {
            //GameObject clone = Instantiate(gameObject) as GameObject;
            //walker w = clone.GetComponent<walker>();
            //w.duration = Random.Range(20f, 50f);
            progress -= 1f;
        }
        //transform.localPosition = spiral.spiralGraph(progress);
        Vector3 position = spline.GetPoint(progress);
        transform.localPosition = position;

        //transform.Rotate(m_RotationDirection * Time.deltaTime * m_Speed);

    }

}
