using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketAnimScript : MonoBehaviour
{

    bool isSideRockets;
    bool isBottomBody;
    bool isTopBody;
    bool isHood;
    bool isVoyager;

    bool voyagerFlying;
    //Vector3 voPos;

    float sideRockets_startTime;
    float bottomBody_startTime;
    float topBody_startTime;
    float hood_startTime;
    float voyager_startTime;

    float duration = 5f;

    private void Start()
    {
        //Anim_TopBody_start();
    }

    // Update is called once per frame
    void Update()
    {
        var timer_sideRockets = (Time.time - sideRockets_startTime) / duration;
        var timer_BottomBody = (Time.time - bottomBody_startTime) / duration;
        var timer_TopBody = (Time.time - topBody_startTime) / duration;
        var timer_Hood = (Time.time - hood_startTime) / duration;
        var timer_Voyager = (Time.time - voyager_startTime) / duration;

        if (isSideRockets)
        {
            var L_rocket = transform.GetChild(3);
            var R_rocket = transform.GetChild(4);

            Vector3 L_pos = L_rocket.position;
            Vector3 R_pos = R_rocket.position;

            Vector3 L_rot = L_rocket.rotation.eulerAngles;
            Vector3 R_rot = R_rocket.rotation.eulerAngles;

            float rocketY = Mathf.SmoothStep(L_pos.y, -25f, timer_sideRockets);
            float rotate_L = Mathf.SmoothStep(L_rot.z, -145f, timer_sideRockets);
            float rotate_R = Mathf.SmoothStep(R_rot.z, 145f, timer_sideRockets);

            L_rocket.position = new Vector3(L_pos.x, rocketY, L_pos.z);
            R_rocket.position = new Vector3(R_pos.x, rocketY, R_pos.z);

            L_rocket.eulerAngles = new Vector3(L_rot.x, L_rot.y, rotate_L);
            R_rocket.eulerAngles = new Vector3(R_rot.x, R_rot.y, rotate_R);


            if (timer_sideRockets >= 0.2f)
            {
                R_rocket.gameObject.SetActive(false);
                L_rocket.gameObject.SetActive(false);
                isSideRockets = false;
            }

        }

        if (isBottomBody)
        {
            var b_body = transform.GetChild(1);
            Vector3 b_body_pos = b_body.position;
            Vector3 b_body_rot = b_body.eulerAngles;

            b_body.position = new Vector3(b_body_pos.x, Mathf.SmoothStep(b_body_pos.y, -25f, timer_BottomBody), b_body_pos.z);
            b_body.eulerAngles = new Vector3(b_body_rot.x, b_body_rot.y, Mathf.SmoothStep(0f, 300f, timer_BottomBody)); // 

            if (timer_BottomBody >= 0.25f)
            {
                transform.GetChild(7).GetChild(0).gameObject.SetActive(true);
                b_body.gameObject.SetActive(false);
            }
        }

        if (isTopBody)
        {
            var TopBody = transform.GetChild(7);
            Vector3 TopBody_pos = TopBody.position;
            Vector3 TopBody_rot = TopBody.eulerAngles;

            TopBody.position = new Vector3(TopBody_pos.x, Mathf.SmoothStep(TopBody_pos.y, -25f, timer_TopBody), TopBody_pos.z);
            TopBody.eulerAngles = new Vector3(TopBody_rot.x, TopBody_rot.y, Mathf.SmoothStep(0f, 300f, timer_TopBody)); // 

            if (timer_TopBody >= 0.25f)
            {
                transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                TopBody.gameObject.SetActive(false);
            }
        }

        if (isHood)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            var L_hood = transform.GetChild(5);
            var R_hood = transform.GetChild(6);

            Vector3 L_pos = L_hood.position;
            Vector3 R_pos = R_hood.position;

            Vector3 L_rot = L_hood.rotation.eulerAngles;
            Vector3 R_rot = R_hood.rotation.eulerAngles;

            float hoodY = Mathf.SmoothStep(L_pos.y, -25f, timer_Hood);
            float L_hoodX = Mathf.SmoothStep(L_pos.x, L_pos.x + 4f, timer_Hood);
            float R_hoodX = Mathf.SmoothStep(R_pos.x, R_pos.x - 4f, timer_Hood);

            float rotate_L = Mathf.SmoothStep(L_rot.x, 145f, timer_Hood);
            float rotate_R = Mathf.SmoothStep(R_rot.x, 145f, timer_Hood);

            L_hood.position = new Vector3(L_hoodX, hoodY, L_pos.z);
            R_hood.position = new Vector3(R_hoodX, hoodY, R_pos.z);

            L_hood.eulerAngles = new Vector3(rotate_L, L_rot.y, L_rot.z);
            R_hood.eulerAngles = new Vector3(rotate_R, R_rot.y, L_rot.z);


            if (timer_Hood >= 0.25f)
            {
                R_hood.gameObject.SetActive(false);
                L_hood.gameObject.SetActive(false);
                isHood = false;
            }
        }

        if (isVoyager)
        {


            var pos = Camera.main.transform.position;
            //var rot = transform.eulerAngles;
            //var size = transform.localScale;

            Camera.main.transform.position = new Vector3(Mathf.SmoothStep(pos.x, -1.61f, timer_Voyager), Mathf.SmoothStep(pos.y, 6.5f, timer_Voyager), Mathf.SmoothStep(pos.z, -0.88f, timer_Voyager));
            ////transform.rotation = Quaternion.Euler( new Vector3(0f, Mathf.SmoothStep(rot.y, -30f, timer_Voyager), 0f));
            //transform.localScale = new Vector3(Mathf.SmoothStep(size.x, 9f, timer_Voyager), Mathf.SmoothStep(size.y, 9f, timer_Voyager), Mathf.SmoothStep(size.z, 9f, timer_Voyager));

            var cent = transform.GetChild(2);
            Vector3 cent_pos = cent.localPosition;
            Vector3 cent_rot = cent.eulerAngles;

            cent.localPosition = new Vector3(cent_pos.x, Mathf.SmoothStep(cent_pos.y, -25f, timer_Voyager), cent_pos.z);
            cent.eulerAngles = new Vector3(cent_rot.x, cent_rot.y, Mathf.SmoothStep(0f, 300f, timer_Voyager));

            if (timer_Voyager >= 0.15f)
            {
                voyagerFlying = true;
                cent.gameObject.SetActive(false);
            }
            if (timer_Voyager >= 1f)
            {

                isVoyager = false;
            }
        }

        if (voyagerFlying)
        {
            transform.GetChild(0).Rotate(Vector3.up * Time.deltaTime * 4f, Space.World);
            //transform.GetChild(0).position = new Vector3(voPos.x, 0.5f * Mathf.Sin(Time.time * Mathf.PI * 0.1f), voPos.z);
        }

    }

    public void Anim_SideRockets_start()
    {
        sideRockets_startTime = Time.time;
        isSideRockets = true;
        Anim_Hood_start();
    }

    public void Anim_BottomBody_start()
    {
        bottomBody_startTime = Time.time;
        isBottomBody = true;

    }

    public void Anim_TopBody_start()
    {
        topBody_startTime = Time.time;
        isTopBody = true;
    }

    public void Anim_Hood_start()
    {
        hood_startTime = Time.time;
        isHood = true;
    }

    public void Anim_Voyager_start()
    {
        voyager_startTime = Time.time;
        isVoyager = true;
        GetComponent<Animator>().SetInteger("state", 1);
    }


}
