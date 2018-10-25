using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundWave2 : MonoBehaviour {

    float[] samples;
    List<float> data = new List<float>();
    LineRenderer lr;

    public Material mat;





	void Start () {
        
        lr = gameObject.GetComponent<LineRenderer>();
        AudioSource aud = GetComponent<AudioSource>();
        samples = new float[aud.clip.samples];
        aud.clip.GetData(samples, 0);


        for (int i = 0; i < samples.Length; i += 10000)
        {
            data.Add(samples[i]);
        }


        lr.positionCount = data.Count;





        //print(orbs.Count);
        //putSoundOnSpline();
    }
	
	
	void Update () {

        for (int i = 0; i < data.Count; i++)
        {

            lr.SetPosition(i, transform.TransformPoint(new Vector3(i * 0.8f, 150f * data[i], 0f)));

        }

        //for (int i = 0; i < orbs.Count; i++)
        //{
        //    lr.SetPosition(i, orbs[i].transform.position);
        //}
	}

    //void putSoundOnSpline()
    //{
    //    for (int i = 1; i < circles.Count; i++)
    //    {
    //        float step = Mathf.Lerp(0f, 0.02f, (float)i / (float)circles.Count);
    //        Vector3 pointOnSpiral = spiral.spiralGraph(step);
    //        circles[i].transform.position = pointOnSpiral;

    //        Vector3 targetDeg = pointOnSpiral - Vector3.zero;
    //        Quaternion rotation = Quaternion.LookRotation(targetDeg, Vector3.left);
    //        circles[i].transform.rotation = rotation;
    //    }

    //    //Vector3 targetDeg = copy.transform.position - Vector3.zero;
    //    //Quaternion rotation = Quaternion.LookRotation(targetDeg);
    //    //copy.transform.rotation = rotation;
    //}
}
