using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBangScript : MonoBehaviour {

    public Vector3 destination;
    Vector3 origin;

    //public GameObject cam;
    float startTime;
    float moveDuration = 0.75f;
    float startSinDuration = 0.45f;
    bool finishedMove = false;

    // User Inputs
    float degreesPerSecond = 15.0f;
    float amplitude = 0f;
    float frequency=0f;
    float finalAmplitude;
    float finalFrequency;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

	// Use this for initialization
	void Start () {
        startTime = Time.time;
        origin = Vector3.zero;

        finalAmplitude = Random.Range(0.05f, 0.1f);
        finalFrequency = Random.Range(0.05f, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(Camera.main.transform.position, Vector3.up);

        if (!finishedMove){
            moveForward();
        }

        if (finishedMove){
            upanddown();

        }
	}

    void moveForward(){
        float t = (Time.time - startTime) / moveDuration;
        transform.position = new Vector3(Mathf.SmoothStep(origin.x, destination.x, t), Mathf.SmoothStep(origin.y, destination.y, t), Mathf.SmoothStep(origin.z, destination.z, t));
        if(t >= 1.0f){
            posOffset = transform.position;
            startTime = Time.time;
            finishedMove = true;
        }
    }

    void upanddown(){
        float t = (Time.time - startTime) / startSinDuration;
        frequency = Mathf.SmoothStep(0f, finalFrequency, t);
        amplitude = Mathf.SmoothStep(0f, finalAmplitude, t);
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }

}
