using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loaderAnimation : MonoBehaviour
{

    public GameObject circleOBJ;
    public Material mat;

    // Use this for initialization
    void Start()
    {

        int randNum = Random.Range(10, 18);

        for (int i = 0; i < randNum; i++)
        {

            float randSize = Random.Range(0.1f, 12f);

            var copy = Instantiate<GameObject>(circleOBJ);
            Vector3 eul = new Vector3(90f, 90f, 90f);
            copy.transform.eulerAngles = eul;
            copy.transform.localScale = new Vector3(randSize, randSize, randSize);
            copy.transform.SetParent(gameObject.transform);

            GameObject orbit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            float randomOrbSize = Random.Range(0.1f, 1.2f);
            orbit.transform.localScale = new Vector3(randomOrbSize, randomOrbSize, randomOrbSize);
            orbit.GetComponent<Renderer>().material = mat;
            var w = orbit.AddComponent<walker>();
            w.spline = copy.GetComponent<BezierSpline>();
            w.duration = Random.Range(6f, 20f);
            orbit.transform.SetParent(gameObject.transform);

            TrailRenderer tr = orbit.AddComponent<TrailRenderer>();
            tr.startWidth = 0.15f;
            tr.endWidth = 0f;
            tr.time = 9f;
            tr.material = mat;
        }

        //int scndRandNum = Random.Range(2, 11);

        //for (int i = 0; i < scndRandNum; i++)
        //{

        //    float scndRandSize = Random.Range(3f, 11f);

        //    var copy = Instantiate<GameObject>(circleOBJ);
        //    Vector3 eul = new Vector3(90f, 90f, 90f);
        //    copy.transform.eulerAngles = eul;
        //    copy.transform.localScale = new Vector3(scndRandSize, scndRandSize, scndRandSize);

        //    GameObject orbit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    float randomOrbSize = Random.Range(0.1f, 1.1f);
        //    orbit.transform.localScale = new Vector3(randomOrbSize, randomOrbSize, randomOrbSize);
        //    orbit.GetComponent<Renderer>().material = mat;
        //    var w = orbit.AddComponent<walker>();
        //    w.spline = copy.GetComponent<BezierSpline>();
        //    w.duration = Random.Range(10f, 30f);

        //    TrailRenderer tr = orbit.AddComponent<TrailRenderer>();
        //    tr.startWidth = 0.1f;
        //    tr.endWidth = 0f;
        //    tr.time = 9f;
        //    tr.material = mat;
        //}
        //GameObject sun = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //float randomSunSize = Random.Range(2.2f, 3.5f);
        //sun.transform.localScale = new Vector3(randomSunSize, randomSunSize, randomSunSize);
        //sun.GetComponent<Renderer>().material = mat;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
