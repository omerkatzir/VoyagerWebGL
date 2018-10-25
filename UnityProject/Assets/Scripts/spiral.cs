using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class spiral
{

    public static Vector3 spiralGraph(float startR, float endR, float startDeg, float endDeg, float startY, float endY, float step)
    {
        step = Mathf.Clamp01(step);

        //float j = Mathf.Lerp(startR, endR, step);
        //float r = 6f * Mathf.Sin(j);
        //float y = Mathf.SmoothStep(startY, endY, step);

        float r = Mathf.Lerp(startR, endR, step);
        float t = Mathf.Lerp(startDeg, endDeg, step);
        float y = Mathf.Lerp(startY, endY, step);

        float x = r * Mathf.Cos(t);
        float z = r * Mathf.Sin(t);

        Vector3 point = new Vector3(x, y, z);

        return point;

    }

    public static Vector3 ballGraph(float startR, float endR, float startDeg, float endDeg, float startY, float endY, float step)
    {
        step = Mathf.Clamp01(step);

        float j = Mathf.Lerp(startR, endR, step);
        float r = 10f * Mathf.Sin(j);
        float t = Mathf.Lerp(startDeg, endDeg, step);
        float y = Mathf.Lerp(startY, endY, step); //smoothstep this to achieve round ball

        float x = r * Mathf.Cos(t);
        float z = r * Mathf.Sin(t);

        Vector3 point = new Vector3(x, y, z);

        return point;

    }

    public static float currentSpiralDeg(float startDeg, float endDeg, float step)
    {
        float t = Mathf.Lerp(startDeg, endDeg, step);
        return t;
    }
}
