using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundWave : MonoBehaviour
{

    float[] samples;
    List<float> data = new List<float>();
    LineRenderer lr;

    public GameObject circleOBJ;
    public Material mat;
    public Material playedMat;
    public List<GameObject> orbs = new List<GameObject>();
    public List<GameObject> circles = new List<GameObject>();
    public AudioSource aud;
    public float song_t;


    private void Awake()
    {

        lr = gameObject.GetComponent<LineRenderer>();
        aud = GetComponent<AudioSource>();

        //float songLength = aud.clip.length;
        //aud.time = songLength / 2f;
        //aud.Play();

        samples = new float[aud.clip.samples];
        aud.clip.GetData(samples, 0);

        print(samples.Length);

        for (int i = 0; i < samples.Length; i += 5000)
        {
            //if (i%10000 == 0){
            data.Add(samples[i]);
            //}

        }


        //for (int i = 0; i < lr.positionCount; i++){
        //    Vector3 nextSpot = transform.localPosition + new Vector3(i * 0.3f, data[i] * 55f, 0f);//Random.Range(-5f, 5f));
        //    //GameObject o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    //o.transform.position = nextSpot;
        //    //float scalerand = Random.Range(0.4f, 1.5f);
        //    //o.transform.localScale = new Vector3(scalerand, scalerand, scalerand);
        //    ////points.Add(nextSpot);
        //    //lr.SetPosition(i, nextSpot);

        //    float randomSize = Random.Range(1f, 2f);
        //    Vector3 orbSize = new Vector3(randomSize, randomSize, randomSize);

        //    GameObject circleClone = Instantiate(circleOBJ) as GameObject;
        //    //float randomRadios = Random.Range(0.07f, 0.3f);
        //    //circleClone.transform.localScale = new Vector3(3f,3f,3f);

        //    GameObject orbit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    orbit.GetComponent<Renderer>().material = mat;
        //    orbit.transform.localScale = orbSize;
        //    orbs.Add(orbit);
        //    walker wCode = orbit.AddComponent(typeof(walker)) as walker;
        //    wCode.spline = circleClone.GetComponent<BezierSpline>();
        //    wCode.duration = Random.Range(6f, 18f);
        //    circleClone.transform.position = nextSpot;
        //}

        //print(data.Count);

        for (int i = 0; i < data.Count; i++)
        {
            if (data[i] > 0f)
            {
                GameObject o = Instantiate<GameObject>(circleOBJ);
                //LineRenderer olr = o.AddComponent<LineRenderer>();
                //olr.material = mat;
                //BezierSpline bc = o.GetComponent<BezierSpline>();

                //olr.positionCount = 36;
                //olr.widthMultiplier =0.15f;



                GameObject orbit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orbit.transform.SetParent(gameObject.transform);
                orbit.AddComponent<OrbitPartOfTrack>();
                //mpott.mat = playedMat;
                TrailRenderer tr = orbit.AddComponent<TrailRenderer>();
                tr.startWidth = 0.3f;
                tr.endWidth = 0f;
                tr.time = 9f;
                tr.material = mat;

                var col = orbit.GetComponent<SphereCollider>();
                col.radius = 5f;

                float randomSize = Random.Range(0.8f, 3f);
                Vector3 oSize = new Vector3(randomSize, randomSize, randomSize);
                orbit.transform.localScale = oSize;
                orbit.GetComponent<Renderer>().material = mat;
                walker wCode = orbit.AddComponent(typeof(walker)) as walker;
                wCode.spline = o.GetComponent<BezierSpline>();
                wCode.duration = Random.Range(6f, 18f);
                o.transform.position = Vector3.right * i * 0.33f;
                o.transform.localScale = new Vector3(100f * (Mathf.Clamp01(data[i])), 100f * (Mathf.Clamp01(data[i])), 100f * (Mathf.Clamp01(data[i])));
                o.AddComponent<CirclePartOfTrack>();
                //for (int f = 0; f < olr.positionCount; f++)
                //{

                //    Vector3 point = bc.GetPoint((float)f / 36f);
                //    olr.SetPosition(f, point);
                //}
                o.transform.parent = this.transform;
                circles.Add(o);
                orbs.Add(orbit);
            }

        }
        lr.positionCount = orbs.Count;

    }

    void Start()
    {
        for (int i = 0; i < orbs.Count; i++)
        {
            circles[i].GetComponent<CirclePartOfTrack>().part = (float)i / (float)orbs.Count;
            orbs[i].GetComponent<OrbitPartOfTrack>().part = (float)i / (float)orbs.Count;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) || Input.touchCount > 2)
        {
            aud.Play();
        }
        song_t = (aud.time / aud.clip.length);

        //print(song_t);

        //FindObjectsOfType<myPartOfTheTrack>()

        //if (aud.isPlaying)
        //{
        //foreach(GameObject o in orbs){
        //    var part = o.GetComponent<myPartOfTheTrack>().part;
        //    if(part<=t){

        //    }
        //}
        //for (int i = 0; i < orbs.Count; i++)
        //{
        //    float t = (float)i / (float)orbs.Count;
        //    print(t);
        //    if (t <= song_t)
        //    {
        //        orbs[i].GetComponent<Renderer>().material = playedMat;
        //        orbs[i].GetComponent<TrailRenderer>().material = playedMat;
        //    }
        //}
        //}

        //foreach (GameObject o in orbs){
        //    o.transform.Rotate(Vector3.right, Random.Range(0.2f,7f));
        //}

        for (int i = 0; i < orbs.Count; i++)
        {
            //    //Vector3 nextSpot = Vector3.Slerp(transform.localPosition + new Vector3(i * 0.12f, 0f, 0f), points[i], step);
            lr.SetPosition(i, orbs[i].transform.position);
        }

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.parent == gameObject.transform)
                {
                    //print("hey");
                    float part = hit.transform.gameObject.GetComponent<OrbitPartOfTrack>().part;
                    song_t = part;

                }

            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.parent == gameObject.transform)
                {
                    float part = hit.transform.gameObject.GetComponent<OrbitPartOfTrack>().part;
                    song_t = part;
                    aud.time = part * aud.clip.length;
                    aud.Play();
                }
            }
        }
    }
}
