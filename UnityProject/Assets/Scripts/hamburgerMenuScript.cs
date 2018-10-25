using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hamburgerMenuScript : MonoBehaviour
{

    float duration = 0.35f;
    public GameObject panel;
    public bool isOpen = false;
    public float startTime;
    float timer;
    Vector2 cur_pos;
    public bool startTimer = false;

    public bool isDisappearing;

    public CategoryButtonScript lastClickedButton;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        cur_pos = panel.GetComponent<RectTransform>().anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDisappearing)
        {
            timer = (Time.time - startTime) / duration;
            Dissappear();
        }

        if (startTimer)
        {
            timer = (Time.time - startTime) / duration;
            if (!isOpen)
            {
                panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(Mathf.SmoothStep(0f, -400f, timer), cur_pos.y);
                if (lastClickedButton != null)
                {
                    lastClickedButton.timer = timer;
                    lastClickedButton.PanelMover();
                }

                //if (lastClickedPanel != null)
                //{
                //    lastClickedPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(Mathf.SmoothStep(0f, -400f, timer), cur_pos.y);
                //}
            }
            else
            {
                panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(Mathf.SmoothStep(-400f, 0f, timer), cur_pos.y);
            }
        }



        if (timer >= 1f)
        {
            if (!isOpen)
            {
                lastClickedButton = null;
            }
            //lastClickedButton = null;
            startTimer = false;

            //if (isOpen)
            //{
            //    isOpen = false;
            //}
            //else
            //{
            //    isOpen = true;
            //}
        }
    }

    IEnumerator CloseCat()
    {
        var cbs = FindObjectsOfType<CategoryButtonScript>();

        yield return new WaitForSeconds(1);
        foreach (var b in cbs)
        {
            if (b.up == true)
            {
                b.TextChange();
                b.startTime = Time.time;
                b.movingDown = true;
            }
        }

        //return;
    }

    void Dissappear()
    {
        var imgcolor = gameObject.GetComponent<Image>().color;
        imgcolor.a = Mathf.Lerp(1f, 0f, timer);
        gameObject.GetComponent<Image>().color = imgcolor;
        if (timer >= 1f)
        {
            isDisappearing = false;
            gameObject.SetActive(false);
        }
    }

    public void OnClick()
    {

        FindObjectOfType<CreditsScript>().gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "i";
        FindObjectOfType<CreditsScript>().onCredits = false;
        FindObjectOfType<CreditsScript>().creditsText.SetActive(false);

        startTime = Time.time;
        startTimer = true;

        if (isOpen)
        {
            isOpen = false;
            //if (lastClickedButton != null)
            //{
            //    lastClickedButton.
            //    lastClickedButton.PanelMover();
            //}

            //lastClickedButton = null;
            StartCoroutine(CloseCat());

            var buttons = FindObjectsOfType<trackListButtonScript>();
            foreach (var t in buttons)
            {
                //if (t != this)
                //{
                if (t.isSelected)
                {
                    t.isSelected = false;
                    t.pressTime = Time.time;
                    t.isPressAnim = true;
                }
                //}
            }
        }
        else
        {
            isOpen = true;
            var tis = FindObjectsOfType<TrackInfoScript>();
            foreach (var info in tis)
            {
                if (info.gameObject.activeInHierarchy)
                {
                    info.HideUI();
                }

            }
        }
    }
}
