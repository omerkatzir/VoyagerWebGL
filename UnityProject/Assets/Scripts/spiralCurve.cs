using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiralCurve : MonoBehaviour {

    public int numOfPoints = 360;
    public Vector3[] spiralPoints;

    public float startR = 5f;
    public float endR = 12f;

    public float startDeg = 0f;
    public float endDeg = 2f * Mathf.PI * 6.02f;

    public float startY = 50f;
    public float endY = 0f;

        
    public Vector3 spiralGraph(float g){

        return transform.TransformPoint(spiral.spiralGraph(startR, endR, startDeg, endDeg, startY, endY, g));
                                        
    //    g = Mathf.Clamp01(g);
    //    float r = Mathf.Lerp(startR, endR, g);
    //    float t = Mathf.Lerp(startDeg, endDeg, g);
    //    float y = Mathf.Lerp(startY, endY, g);

    //    float x = r * Mathf.Cos(t);
    //    float z = r * Mathf.Sin(t);

    //    Vector3 point = new Vector3(x, y, z);

    //    return point;

    }

    public float getCurrentDeg(float g){
        return spiral.currentSpiralDeg(startDeg, endDeg, g);
    }



    public Vector3 GetSpiralPoint(int index)
    {
        return spiralPoints[index];
    }

    public void Reset()
    {
        spiralPoints = new Vector3[numOfPoints];
        for (int i=0; i < numOfPoints; i++){
            float whereongraph = (float)i / (float)numOfPoints;
            spiralPoints[i] = spiralGraph(whereongraph);
        }
    }



}
