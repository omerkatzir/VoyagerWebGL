using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButtonScript : MonoBehaviour
{
    Vector2 cur_pos;

    public GameObject[] otherButtons = new GameObject[3];
    public GameObject panel;

    public float startTime;
    float colorDuration = 0.25f;// * 1.3f;
    float duration = 0.6f;// * 1.3f;
    float final_y = 0f;
    public bool movingUp = false;
    public bool movingDown = false;
    public bool up = false;
    public float timer;
    float colorTimer;

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        cur_pos = gameObject.GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        if (movingUp || movingDown)
        {
            timer = (Time.time - startTime) / duration;
            colorTimer = (Time.time - startTime) / colorDuration;
        }

        if (movingUp)
        {
            float smoothY = Mathf.SmoothStep(cur_pos.y, final_y, timer);
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(cur_pos.x, smoothY);
            foreach (var ob in otherButtons)
            {
                if (ob != this)
                {
                    ob.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), colorTimer);
                    ob.transform.GetChild(1).gameObject.GetComponent<Text>().color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), colorTimer);
                }

            }

            int childrenNum = panel.transform.GetChild(0).childCount;
            for (int i = 0; i < childrenNum; i++)
            {
                var linkButton = panel.transform.GetChild(0).GetChild(i);
                linkButton.GetChild(0).gameObject.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, timer);
                linkButton.GetChild(1).gameObject.GetComponent<Text>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, timer);
                linkButton.GetChild(2).gameObject.GetComponent<Text>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, timer);
                linkButton.GetChild(3).gameObject.GetComponent<Text>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, timer);

                Vector2 panelCurPos = panel.GetComponent<RectTransform>().anchoredPosition;
                float xMovePanel = Mathf.SmoothStep(-750f, 0f, timer);
                panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(xMovePanel, panelCurPos.y);
                panel.SetActive(true);
            }

            up = true;
            if (timer >= 1f)
            {
                movingUp = false;
            }
        }

        if (movingDown)
        {
            float smoothY = Mathf.SmoothStep(final_y, cur_pos.y, timer);
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(cur_pos.x, smoothY);
            //var otherButtons = FindObjectsOfType<CategoryButtonScript>();
            foreach (var ob in otherButtons)
            {
                //if (ob != this)
                //{
                ob.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, colorTimer);
                ob.transform.GetChild(1).gameObject.GetComponent<Text>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, colorTimer);
                //}

            }

            PanelMover();

            up = false;
            if (timer >= 1f)
            {
                movingDown = false;
            }
        }

    }

    public void PanelMover()
    {
        int childrenNum = panel.transform.GetChild(0).childCount;
        for (int i = 0; i < childrenNum; i++)
        {
            var linkButton = panel.transform.GetChild(0).GetChild(i);
            linkButton.GetChild(0).gameObject.GetComponent<Image>().color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), timer);
            linkButton.GetChild(1).gameObject.GetComponent<Text>().color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), timer);
            linkButton.GetChild(2).gameObject.GetComponent<Text>().color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), timer);
            linkButton.GetChild(3).gameObject.GetComponent<Text>().color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), timer);
        }
        Vector2 panelCurPos = panel.GetComponent<RectTransform>().anchoredPosition;
        float xMovePanel = Mathf.SmoothStep(0f, -750f, timer);
        panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(xMovePanel, panelCurPos.y);

        if (timer >= 1f)
        {
            panel.SetActive(false);
        }
    }

    public void TextChange()
    {
        if (!up)
        {
            if (transform.GetChild(0).gameObject.GetComponent<Text>().text == "A\\ Pictures")
            {
                //print("hey!");
                transform.GetChild(0).gameObject.GetComponent<Text>().text = "</" + " Pictures";
            }
            if (transform.GetChild(0).gameObject.GetComponent<Text>().text == "B\\ Greetings")
            {

                transform.GetChild(0).gameObject.GetComponent<Text>().text = "</" + " Greetings";
            }
            if (transform.GetChild(0).gameObject.GetComponent<Text>().text == "C\\ Sounds")
            {
                transform.GetChild(0).gameObject.GetComponent<Text>().text = "</" + " Sounds";
            }
            if (transform.GetChild(0).gameObject.GetComponent<Text>().text == "D\\ Music")
            {
                transform.GetChild(0).gameObject.GetComponent<Text>().text = "</" + " Music";
            }
        }
        else
        {
            if (transform.GetChild(0).gameObject.GetComponent<Text>().text == "</ Pictures")
            {
                transform.GetChild(0).gameObject.GetComponent<Text>().text = "A\\ Pictures";
            }
            if (transform.GetChild(0).gameObject.GetComponent<Text>().text == "</ Greetings")
            {
                transform.GetChild(0).gameObject.GetComponent<Text>().text = "B\\ Greetings";
            }
            if (transform.GetChild(0).gameObject.GetComponent<Text>().text == "</ Sounds")
            {
                transform.GetChild(0).gameObject.GetComponent<Text>().text = "C\\ Sounds";
            }
            if (transform.GetChild(0).gameObject.GetComponent<Text>().text == "</ Music")
            {
                transform.GetChild(0).gameObject.GetComponent<Text>().text = "D\\ Music";
            }
        }




    }

    void OnClick()
    {
        TextChange();

        startTime = Time.time;
        if (!up)
        {
            movingUp = true;
            FindObjectOfType<hamburgerMenuScript>().lastClickedButton = this;

        }
        else
        {
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

            movingDown = true;
        }


    }
}
