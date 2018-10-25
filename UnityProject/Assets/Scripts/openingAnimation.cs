using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openingAnimation : MonoBehaviour
{
    public Material mat;
    public GameObject circle;
    public float numberOfStars = 800f;
    public float scaleMin = 0.1f;
    public float scaleMax = 0.1f;
    public float minRand = 0.5f;
    public float maxRand = 15f;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < numberOfStars; i++)
        {
            GameObject star = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            star.GetComponent<Renderer>().material = mat;
            GameObject circleCopy = Instantiate<GameObject>(circle);
            var scale = Random.Range(scaleMin, scaleMax);
            star.transform.localScale = new Vector3(scale, scale, scale);
            circleCopy.transform.position = transform.TransformPoint(gameObject.GetComponent<ballSpiral>().ballGraph(i / numberOfStars));
            var randSize = Random.Range(minRand, maxRand);
            circleCopy.transform.localScale = new Vector3(randSize, randSize, randSize);
            var w = star.AddComponent<walker>();
            w.spline = circleCopy.GetComponent<BezierSpline>();
            w.duration = Random.Range(55f, 85f);
            //circleCopy.transform.SetParent(transform);
            //star.transform.SetParent(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
