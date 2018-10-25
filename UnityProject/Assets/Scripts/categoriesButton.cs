using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class categoriesButton : MonoBehaviour
{

    int state = 0;
    GameObject hamManu;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        hamManu = FindObjectOfType<hamburgerMenuScript>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0)
        {
            transform.GetChild(1).gameObject.GetComponent<Text>().text = "Chronological";
        }
        if (state == 1)
        {
            transform.GetChild(1).gameObject.GetComponent<Text>().text = "By Categories";
        }
    }

    void OnClick()
    {
        FindObjectOfType<CreditsScript>().gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "i";
        FindObjectOfType<CreditsScript>().onCredits = false;
        FindObjectOfType<CreditsScript>().creditsText.SetActive(false);

        if (!FindObjectOfType<MusicOrganizer>().firstAnim && !FindObjectOfType<MusicOrganizer>().isSpiralOpening)
        {
            if (state == 0)
            {
                state = 1;
                FindObjectOfType<MusicOrganizer>().CategoryMover();
                if (hamManu.GetComponent<hamburgerMenuScript>().isOpen)
                {
                    hamManu.GetComponent<hamburgerMenuScript>().OnClick();
                }

                hamManu.GetComponent<hamburgerMenuScript>().startTime = Time.time;
                hamManu.GetComponent<hamburgerMenuScript>().isDisappearing = true;

                //hamManu.SetActive(false);

            }
            else
            {
                state = 0;
                FindObjectOfType<MusicOrganizer>().CategoryMover();
                hamManu.SetActive(true);
                hamManu.GetComponent<Image>().color = Color.white;
                FindObjectOfType<MusicOrganizer>().inACategory = false;
            }

            var tr = FindObjectsOfType<TrailRenderer>();
            foreach (var trenderer in tr)
            {
                trenderer.enabled = false;
            }
            foreach (Transform trans in FindObjectOfType<MusicOrganizer>().uiPanel_audio.transform)
            {
                trans.gameObject.SetActive(false);
            }
        }


        //print(state);
    }
}
