using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleInstantiator : MonoBehaviour
{

    public BezierSpline bs;
    public GameObject circleOBJ;
    public List<GameObject> orbs;
    //List<Vector3> orbs_pos;
    public Material mat;


    public int numberOfCircles = 70;
    LineRenderer lr;

    // Use this for initialization
    void Start()
    {

        lr = gameObject.GetComponent<LineRenderer>();
        orbs = new List<GameObject>();


        for (int i = 0; i <= numberOfCircles; i++)
        {
            float randomSize = Random.Range(0.05f, 0.2f);
            Vector3 orbSize = new Vector3(randomSize, randomSize, randomSize);

            GameObject circleClone = Instantiate(circleOBJ) as GameObject;
            //float randomRadios = Random.Range(0.07f, 0.3f);
            //circleClone.transform.localScale = new Vector3(randomRadios,randomRadios,randomRadios);
            //circleClone.transform.localScale = Vector3.zero;

            GameObject orbit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            orbs.Add(orbit);
            orbit.GetComponent<Renderer>().material = mat;
            orbit.transform.localScale = orbSize;
            walker wCode = orbit.AddComponent(typeof(walker)) as walker;
            wCode.spline = circleClone.GetComponent<BezierSpline>();
            wCode.duration = Random.Range(6f, 18f);
            circleClone.transform.LookAt(bs.GetPoint((float)i / (float)numberOfCircles) + bs.GetDirection((float)i / (float)numberOfCircles));
            circleClone.transform.position = bs.GetPoint((float)i / (float)numberOfCircles);//+Random.Range(0.0f,0.06f));
        }

        lr.positionCount = orbs.Count;

    }

    // Update is called once per frame
    void Update()
    {

        //foreach (GameObject orbit in orbs){
        //    Vector3 orb_pos = orbit.transform.position;
        //    orbs_pos.Add(orb_pos);
        //}

        for (int i = 0; i < orbs.Count; i++)
        {
            lr.SetPosition(i, orbs[i].transform.position);
        }
    }
}
