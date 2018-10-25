using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class glitchScript : MonoBehaviour
{

    public GameObject ballSpiralOBJ;
    List<GameObject> listOfObjects = new List<GameObject>();
    public Material mat;
    public GameObject circleOBJ;
    public spiralCurve spiral;
    GameObject parent;
    bool justFinished;

    AudioClip[] audiotracks;

    float pointOfAudioTracks;// = 0.0f;
    float maxTrackWidth = 0.9f;

    private void Awake()
    {
        parent = new GameObject("parent");

        audiotracks = Resources.LoadAll("Audio", typeof(AudioClip)).Cast<AudioClip>().ToArray();


        for (int i = 0; i < audiotracks.Length; i++)
        {
            GameObject audioGO = new GameObject("track " + (i + 1));
            audioGO.transform.SetParent(parent.transform);

            audioGO.tag = "AudioTrack";

            AudioSource goas = audioGO.AddComponent<AudioSource>();
            goas.playOnAwake = false;
            goas.clip = audiotracks[i];
            audioGO.AddComponent<Swlists>();

            listOfObjects.Add(audioGO);
            CreateAudioTrack(audioGO, i);

        }
    }


    void CreateAudioTrack(GameObject go, int num)
    {
        Swlists sw = go.GetComponent<Swlists>();

        List<GameObject> tempCircles = new List<GameObject>();
        List<GameObject> tempOrbits = new List<GameObject>();

        AudioSource aud = go.GetComponent<AudioSource>();


        float[] samples = new float[aud.clip.samples];
        List<float> data = new List<float>();

        aud.clip.GetData(samples, 0);

        int numOfData = samples.Length / 950;

        for (int i = 0; i < samples.Length; i += numOfData)
        {

            data.Add(samples[i]);
        }

        for (int i = 0; i < data.Count; i++)
        {

            if (data[i] > 0f)
            {


                GameObject circle = Instantiate<GameObject>(circleOBJ);
                GameObject orbit = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                orbit.name = "orbit " + i;

                orbit.GetComponent<SphereCollider>().radius = 8f;

                TrailRenderer tr = orbit.AddComponent<TrailRenderer>();
                tr.startWidth = 0.3f;
                tr.endWidth = 0f;
                tr.time = 4f;
                tr.material = mat;
                //tr.enabled = false;

                float randomSize = Random.Range(0.8f, 2.5f);
                Vector3 oSize = new Vector3(randomSize, randomSize, randomSize);
                orbit.transform.localScale = oSize;

                orbit.GetComponent<Renderer>().material = mat;
                walker wCode = orbit.AddComponent<walker>();
                wCode.spline = circle.GetComponent<BezierSpline>();
                wCode.duration = Random.Range(6f, 20f);
                wCode.progress = Random.Range(0f, 1f);

                circle.transform.parent = go.transform;
                orbit.transform.parent = go.transform;

                circle.AddComponent<objectMover>();

                circle.transform.localScale = new Vector3(130f * (Mathf.Clamp01(data[i])), 130f * (Mathf.Clamp01(data[i])), 130f * (Mathf.Clamp01(data[i])));


                //listOfObjects.Add(circle);
                tempCircles.Add(circle);
                tempOrbits.Add(orbit);

            }

        }
        for (int i = 0; i < tempCircles.Count; i++)
        {

            float step = Mathf.Lerp((float)num / (float)audiotracks.Length, (float)(num + 1) / (float)audiotracks.Length, maxTrackWidth * ((float)i / (float)tempCircles.Count));
            float afterPicsStep = Mathf.Lerp(pointOfAudioTracks, 1f, step);

            Vector3 pointOnSpiral = ballSpiralOBJ.GetComponent<ballSpiral>().ballGraph(afterPicsStep);// go.transform.InverseTransformPoint(spiral.spiralGraph(afterPicsStep));

            MyPointOnGraph mpog = tempCircles[i].AddComponent<MyPointOnGraph>();
            mpog.point = afterPicsStep;

            tempCircles[i].transform.position = pointOnSpiral;

            Vector3 targetDeg = pointOnSpiral - Vector3.zero; //go.transform.TransformPoint(pointOnSpiral) - Vector3.zero;
            Quaternion rotation = Quaternion.LookRotation(targetDeg, Vector3.left);
            tempCircles[i].transform.rotation = rotation;

        }

        sw.orbitObjects.AddRange(tempOrbits);
        sw.circleObjects.AddRange(tempCircles);
    }

    private void Update()
    {
        //parent.transform.Rotate(Vector3.right, 0.5f);

        if (Input.GetMouseButtonUp(0))
        {
            if (!justFinished)
            {
                var trs = FindObjectsOfType<TrailRenderer>();
                foreach (var t in trs)
                {
                    t.Clear();
                }
                for (int i = 0; i < listOfObjects.Count; i++)
                {
                    var rndPos = Random.insideUnitSphere * 1000f;
                    var circ = listOfObjects[i].GetComponent<Swlists>().circleObjects;
                    listOfObjects[i].GetComponent<Swlists>().startTime = Time.time;
                    listOfObjects[i].GetComponent<Swlists>().isTimerRunning = true;
                    listOfObjects[i].GetComponent<Swlists>().isStartingTrail = true;

                    foreach (GameObject c in circ)
                    {
                        var om = c.GetComponent<objectMover>();
                        if (om.oldPos == Vector3.zero)
                        {
                            om.GetOldPos();
                        }
                        //om.duration = Random.Range(1f, 4f);
                        om.newPos = rndPos; //Random.insideUnitSphere * 1000f;
                        om.startTime = Time.time;
                        om.isMoving = true;
                    }

                }
                justFinished = true;
            }
            else
            {
                var trs = FindObjectsOfType<TrailRenderer>();
                foreach (var t in trs)
                {
                    t.Clear();
                }

                for (int i = 0; i < listOfObjects.Count; i++)
                {
                    var circ = listOfObjects[i].GetComponent<Swlists>().circleObjects;
                    foreach (GameObject c in circ)
                    {
                        var om = c.GetComponent<objectMover>();
                        c.transform.position = om.oldPos;
                    }

                }
                justFinished = false;
            }

            //    var circ = g.GetComponent<SoundWave>().circleObjects;



            //        var catBallCode = catSpheres[i].GetComponentInChildren<ballSpiral>();
            //        om.newPos = catBallCode.ballGraph((float)d / (float)catMasterCode.categories[i].Count);




        }
    }

}

public class Swlists : MonoBehaviour
{
    public List<GameObject> circleObjects = new List<GameObject>();
    public List<GameObject> orbitObjects = new List<GameObject>();

    public float startTime;
    float duration = 2f;
    float timer;

    public bool isTimerRunning = false;
    public bool isStartingTrail = false;

    private void Update()
    {
        if (isTimerRunning)
        {
            timer = (Time.time - startTime) / duration;
        }
        if (isStartingTrail)
        {
            ShowTrail();
        }
    }

    public void ShowTrail()
    {
        foreach (GameObject g in orbitObjects)
        {
            TrailRenderer tr = g.GetComponent<TrailRenderer>();
            float time = 4f;

            //tr.enabled = true;

            tr.time = Mathf.Lerp(0f, time, timer);

        }
        if (timer >= 1f)
        {
            isTimerRunning = false;
            isStartingTrail = false;
        }
    }
}
