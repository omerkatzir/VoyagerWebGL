using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ballSpiral))]
public class BallSpiralInspector : Editor {

    private void OnSceneGUI()
    {
        
        ballSpiral spiral = target as ballSpiral;
        Transform handleTransform = spiral.transform;

        for (int i = 0; i < spiral.spiralPoints.Length - 1; i++){
            Vector3 p0 = handleTransform.TransformPoint(spiral.GetSpiralPoint(i));
            Vector3 p1 = handleTransform.TransformPoint(spiral.GetSpiralPoint(i+1));

            Handles.color = Color.white;
            Handles.DrawLine(p0, p1);
        }

    }


}
