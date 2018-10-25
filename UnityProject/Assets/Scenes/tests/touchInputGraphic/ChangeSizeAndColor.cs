using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSizeAndColor : MonoBehaviour
{
    public bool timerRunning;
    public bool touchStarted;
    public bool touchEnded;

    float duration = 0.2f;
    float timer;
    public float startTime;

    Image img;

    void Start()
    {
        img = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if (timerRunning)
        {
            timer = (Time.time - startTime) / duration;

            ChangeThings();

            if (timer >= 1f)
            {
                //touchStarted = false;
                //touchEnded = false;

                timerRunning = false;

            }
        }
    }

    void ChangeThings()
    {
        if (touchStarted)
        {
            var size = Mathf.Lerp(9.5f, 6f, timer);
            transform.localScale = new Vector3(size, size, 1f);

            Color clr = img.color;
            clr.a = timer;//new Color(1, 1, 1, timer);
            img.color = clr;

            if (timer >= 1f)
            {
                touchStarted = false;
            }
        }

        if (touchEnded)
        {
            var size = Mathf.Lerp(6f, 9.5f, timer);
            transform.localScale = new Vector3(size, size, 1f);

            Color clr = img.color;
            clr.a = 1 - timer; // new Color(1, 1, 1, 1 - timer);
            img.color = clr;

            if (timer >= 1f)
            {
                touchEnded = false;
                gameObject.SetActive(false);
            }

        }
    }
}
