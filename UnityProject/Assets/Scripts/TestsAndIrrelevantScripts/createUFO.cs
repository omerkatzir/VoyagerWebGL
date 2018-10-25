using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createUFO : MonoBehaviour
{

    public GameObject ufoOBJ;
    public ballSpiral bs_1;
    ballSpiral bs_2;

    // Use this for initialization
    void Start()
    {
        bs_2 = GetComponent<ballSpiral>();

        for (int i = 0; i < 100; i++)
        {
            var copyUFO = Instantiate<GameObject>(ufoOBJ);
            var om = copyUFO.GetComponent<objectMover>();

            om.oldPos = bs_1.ballGraph(Random.Range(0f, 1f));
            om.newPos = bs_2.ballGraph(Random.Range(0f, 1f));
            om.startTime = Time.time;
            om.isMoving = true;
            copyUFO.SetActive(true);
        }

        var camom = Camera.main.GetComponent<objectMover>();
        camom.newPos = Camera.main.transform.position;
        camom.oldPos = new Vector3(0f, -1.7f, -6.5f);
        camom.startTime = Time.time;
        camom.isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
