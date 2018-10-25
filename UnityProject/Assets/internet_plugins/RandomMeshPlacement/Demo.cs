using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// To demonstrate the random generation of points within a mesh.
/// 
/// If you assume non-convexity, you should place the child transform named "Center" 
/// somewhere in your mesh from where you can see most of the vertices 
/// </summary>
public class Demo : MonoBehaviour
{

    /// <summary>
    /// Assume Convexity? If not, place the Center as directed.
    /// </summary>
    public bool isConvex = false;
    /// <summary>
    /// Place somewhere inside your mesh, from where you can see most of the vertices.
    /// </summary>
    //Transform center;
    public Material mat;
    public GameObject circle;

    /// <summary>
    /// Rate of spawning - press "T" to instantly spawn more spheres.
    /// </summary>
    public int pointsToAddPerSecond = 50;

    MeshFilter mF;
    List<Point> points;
    //List<Point> morePoints;
    void Start()
    {
        //center = transform.GetChild(0);
        mF = GetComponent<MeshFilter>();
        points = new List<Point>();
        Populate(300);
        foreach (Point p in points)
        {
            GameObject c = Instantiate<GameObject>(circle);
            c.transform.localScale = new Vector3(Random.Range(0.01f, 0.08f), Random.Range(0.01f, 0.08f), Random.Range(0.01f, 0.08f));
            c.transform.position = transform.TransformPoint(p.pos);
            c.transform.eulerAngles = new Vector3(Random.Range(-90f, 90f), Random.Range(-90f, 90f), Random.Range(-90f, 90f));
            GameObject q = GameObject.CreatePrimitive(PrimitiveType.Quad);
            q.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            q.GetComponent<Renderer>().material = mat;
            var w = q.AddComponent<walker>();
            w.duration = Random.Range(5f, 10f);
            w.spline = c.GetComponent<BezierSpline>();

            if (transform.parent != null)
            {
                c.transform.SetParent(transform.parent);
                q.transform.SetParent(transform.parent);
            }

        }
        //for (int i = 0; i < 20; i++)
        //{
        //    GameObject c = Instantiate<GameObject>(circle);
        //    c.transform.localScale = new Vector3(Random.Range(0.01f, 1f), Random.Range(0.01f, 1f), Random.Range(0.01f, 1f));
        //    c.transform.position = new Vector3(Random.Range(-13f, 13f), Random.Range(-1.5f, 2f), Random.Range(-1f, 1f));
        //    c.transform.eulerAngles = new Vector3(Random.Range(-90f, 90f), Random.Range(-90f, 90f), Random.Range(-90f, 90f));
        //    GameObject q = GameObject.CreatePrimitive(PrimitiveType.Quad);
        //    q.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        //    q.GetComponent<Renderer>().material = mat;
        //    var w = q.AddComponent<walker>();
        //    w.duration = Random.Range(35f, 55f);
        //    w.spline = c.GetComponent<BezierSpline>();
        //}

    }

    //void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.T))
    //        Populate(5 * pointsToAddPerSecond, 10);
    //}
    //void FixedUpdate()
    //{
    //    Populate(Mathf.CeilToInt(pointsToAddPerSecond * Time.fixedDeltaTime), 10);
    //}

    void Populate(int numRays, float duration = 0)
    {
        if (duration <= 0)
            duration = Time.deltaTime;
        if (numRays <= 0)
            numRays = 1;

        for (int i = 0; i < numRays; i++)
        {
            Vector3 point = mF.mesh.GetRandomPointOnSurface();
            //Vector3 point = isConvex ? mF.mesh.GetRandomPointInsideConvex() : mF.mesh.GetRandomPointInsideNonConvex(center.localPosition);
            points.Add(new Point(point));
        }
    }

    //IEnumerator DelayedDelete(Point p)
    //{
    //    yield return new WaitForSeconds(10);
    //    points.Remove(p);
    //}

    //void OnDrawGizmos()
    //{
    //    if (points == null || points.Count == 0)
    //        return;

    //    foreach (Point p in points)
    //    {
    //        Gizmos.color = Color.red; // The_Helper.InterpolateColor(Color.red, Color.green, p.pos.magnitude); 

    //        Gizmos.DrawSphere(transform.TransformPoint(p.pos), transform.lossyScale.magnitude / 100);
    //    }
    //}

    struct Point
    {
        public Point(Vector3 pos)
        {
            this.pos = pos;
        }
        public Vector3 pos;
    }
}
