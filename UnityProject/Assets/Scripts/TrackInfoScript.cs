using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackInfoScript : MonoBehaviour
{

    public GameObject myUiObject;
    public bool isImage;
    bool isShowingNow;
    bool isHidingNow;
    float startTime;// = 2f;
    float Duration = 0.8f;

    void Update()
    {
        if (isShowingNow)
        {
            float timer = (Time.time - startTime) / Duration;
            var images = myUiObject.GetComponentsInChildren<Image>();
            foreach (var img in images)
            {
                img.color = new Color(1, 1, 1, Mathf.Lerp(0f, 1f, timer));
            }

            var texts = myUiObject.GetComponentsInChildren<Text>();
            foreach (var txt in texts)
            {
                //txt.gameObject.SetActive(true);
                txt.color = new Color(1, 1, 1, Mathf.Lerp(0f, 1f, timer));
            }

            if (timer >= 1f)
            {
                isShowingNow = false;
            }
        }

        if (isHidingNow)
        {
            float timer = (Time.time - startTime) / Duration;
            var images = myUiObject.GetComponentsInChildren<Image>();
            foreach (var img in images)
            {
                img.color = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, timer * 2f));
            }

            var texts = myUiObject.GetComponentsInChildren<Text>();
            foreach (var txt in texts)
            {
                //txt.gameObject.SetActive(false);
                txt.color = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, timer));
            }

            if (timer >= 1f)
            {
                myUiObject.SetActive(false);
                isHidingNow = false;
            }
        }
    }

    public void ShowUI()
    {
        myUiObject.SetActive(true);
        startTime = Time.time;
        isShowingNow = true;

    }

    public void HideUI()
    {
        startTime = Time.time;
        isHidingNow = true;
        //myUiObject.SetActive(false);
    }
}
