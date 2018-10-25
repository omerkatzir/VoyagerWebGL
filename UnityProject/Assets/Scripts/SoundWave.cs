using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour
{

    public List<Vector3> circlePos = new List<Vector3>();
    public List<GameObject> circleObjects = new List<GameObject>();
    public List<GameObject> orbitObjects = new List<GameObject>();

    public float song_t;
    public AudioSource aud;
    public GameObject midPoint;

    //public GameObject otherParent;

    public bool isScrubbing;

    bool stopTrail = false;
    bool isflat = false;
    public bool isTimerRunning = false;
    public bool isStartingTrail = false;
    public bool isStopingTrail = false;
    public bool isBecomingFlat = false;
    public bool onMe = false;

    public float startTime;
    float duration = 2f;
    float timer;

    AudioSource[] allAS;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        //givePartOfTrack();
        for (int i = 0; i < circleObjects.Count; i++)
        {
            circleObjects[i].GetComponent<CirclePartOfTrack>().part = (float)i / (float)circleObjects.Count;
            orbitObjects[i].GetComponent<OrbitPartOfTrack>().part = (float)i / (float)orbitObjects.Count;
        }

        allAS = FindObjectsOfType<AudioSource>();
    }

    public void stopAllAudio()
    {
        foreach (AudioSource a in allAS)
        {
            if (a != aud)
            {
                a.time = 0;
                a.Stop();
            }
        }
    }

    void Update()
    {
        if (isScrubbing)
        {
            print("scrubbing");
        }

        if (onMe)
        {
            song_t = (aud.time / aud.clip.length);

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

                        isScrubbing = true;

                    }

                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                isScrubbing = false;

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
                    //else
                    //{
                    //    onMe = false;
                    //}
                }

            }
            if (aud.isPlaying)
            {
                stopAllAudio();
            }

        }

        if (isTimerRunning)
        {

            timer = (Time.time - startTime) / duration;
        }

        if (isStopingTrail)
        {
            StopShowTrail();
        }

        if (isStartingTrail)
        {
            ShowTrail();
        }

        if (isBecomingFlat)
        {
            BeFlat();
        }

        if (stopTrail)
        {
            noTrail();
        }
    }

    public void noTrail()
    {
        foreach (GameObject g in orbitObjects)
        {
            TrailRenderer tr = g.GetComponent<TrailRenderer>();
            tr.enabled = false;
        }
        stopTrail = false;
    }

    public void takePositions()
    {

        for (int i = 0; i < circleObjects.Count; i++)
        {
            circlePos.Add(circleObjects[i].transform.position);
        }
    }

    //public void givePartOfTrack()
    //{
    //    for (int i = 0; i < circleObjects.Count; i++)
    //    {
    //        circleObjects[i].GetComponent<CirclePartOfTrack>().part = (float)i / (float)circleObjects.Count;
    //    }
    //    for (int i = 0; i < orbitObjects.Count; i++)
    //    {
    //        orbitObjects[i].GetComponent<OrbitPartOfTrack>().part = (float)i / (float)orbitObjects.Count;
    //    }
    //}

    public void BeFlat()
    {
        Vector3 midPos = midPoint.transform.position;
        for (int i = 0; i < circleObjects.Count; i++)
        {

            if (!isflat)
            {

                Vector3 move = new Vector3(Mathf.SmoothStep(circlePos[i].x, midPos.x, timer), Mathf.SmoothStep(circlePos[i].y, midPos.y, timer), Mathf.SmoothStep(circlePos[i].z, midPos.z, timer));
                circleObjects[i].transform.position = move;
                //circleObjects[i].transform.rotation = circleObjects[(circleObjects.Count / 2)].transform.rotation;

            }
            else
            {
                isStopingTrail = true;
                Vector3 move = new Vector3(Mathf.SmoothStep(midPos.x, circlePos[i].x, timer), Mathf.SmoothStep(midPos.y, circlePos[i].y, timer), Mathf.SmoothStep(midPos.z, circlePos[i].z, timer));
                circleObjects[i].transform.position = move;
            }

        }
        if (timer >= 1f)
        {
            if (!isflat)
            {
                isStartingTrail = true;
                startTime = Time.time;
                isflat = true;
            }
            else
            {
                isflat = false;
            }

            isBecomingFlat = false;
        }
    }

    public void ShowTrail()
    {
        foreach (GameObject g in orbitObjects)
        {
            TrailRenderer tr = g.GetComponent<TrailRenderer>();
            float time = 9f;
            tr.enabled = true;

            tr.time = Mathf.Lerp(0f, time, timer);

        }
        if (timer >= 1f)
        {
            isTimerRunning = false;
            isStartingTrail = false;
        }
    }

    public void StopShowTrail()
    {
        foreach (GameObject g in orbitObjects)
        {
            TrailRenderer tr = g.GetComponent<TrailRenderer>();
            float time = 9f;
            tr.time = Mathf.Lerp(time, 0f, timer);
            if (timer >= 1f)
            {
                tr.enabled = false;
            }
        }
        if (timer >= 1f)
        {
            isTimerRunning = false;
            isStopingTrail = false;
        }

    }

}
