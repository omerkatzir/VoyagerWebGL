using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScript : MonoBehaviour
{
    public GameObject creditsText;
    public bool onCredits;
    Transform camTrans;

    // Use this for initialization
    void Start()
    {
        GameObject creditsCamTrans = new GameObject("creditsCamTrans");
        creditsCamTrans.transform.position = new Vector3(5350f, 0f, -5000f);
        creditsCamTrans.transform.eulerAngles = Vector3.zero;

        camTrans = creditsCamTrans.transform;
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        var mo = FindObjectOfType<MusicOrganizer>();
        if (!mo.firstAnim && !mo.isSpiralOpening) //&& !mo.cameraMoving)
        {
            if (!onCredits)
            {
                if (mo.inACategory)
                {
                    mo.lastCamTrans.root.gameObject.GetComponent<Collider>().enabled = true;
                    mo.inACategory = false;
                }

                creditsText.SetActive(true);

                mo.lastCamTrans = mo.camTrans;
                mo.camTrans = camTrans;

                if (mo.inAudio)
                {
                    mo.LastObjectAudio();
                }
                else if (mo.inImage)
                {
                    mo.lastCamTrans.transform.root.gameObject.GetComponent<TrackInfoScript>().HideUI();
                }

                mo.inImage = false;
                mo.inAudio = false;

                mo.cameraStartTime = Time.time;
                mo.cameraMoving = true;

                gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "X";

                onCredits = true;
            }
            else
            {

                creditsText.SetActive(false);
                mo.lastCamTrans = mo.camTrans;

                mo.camTrans = mo.camOrigin.transform;
                if (mo.inCategories)
                {
                    mo.camTrans = mo.catCamOrigin.transform;
                }

                mo.cameraStartTime = Time.time;
                mo.cameraMoving = true;

                gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "i";

                onCredits = false;
            }

        }


    }
}
