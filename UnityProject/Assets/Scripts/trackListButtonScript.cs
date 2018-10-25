using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class trackListButtonScript : MonoBehaviour
{
    //button
    public Transform camTrans;
    public bool isPressAnim = false;
    public bool isSelected;
    public float pressTime;
    float duration = 0.25f;
    int calc;
    Vector2 rH;
    public Text titleText;
    public Text timeText;
    public Text orderNumText;
    float buttonWidth;

    Animator anim;

    bool isAudio = false;
    public string trackLength;
    public int numTrack;
    public GameObject track;
    MusicOrganizer mo;

    // Use this for initialization
    void Start()
    {
        mo = FindObjectOfType<MusicOrganizer>();

        titleText = transform.GetChild(1).GetComponent<Text>();
        //if(track.)

        orderNumText = transform.GetChild(2).GetComponent<Text>();
        timeText = transform.GetChild(3).GetComponent<Text>();
        giveOrderNum();

        if (track.CompareTag("AudioTrack"))
        {
            titleText.text = mo.tracknames[numTrack];
            timeText.text = trackLength;
            isAudio = true;
        }
        else
        {
            titleText.text = mo.imageNames[numTrack];
            timeText.enabled = false;
            //print("i'm a pic, yo!");
        }


        rH = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void giveOrderNum()
    {
        if (track.CompareTag("AudioTrack"))
        {
            if (numTrack < 3)
            {
                orderNumText.text = "B.0" + (numTrack + 1);
            }
            if (numTrack == 3)
            {
                orderNumText.text = "C.01";
            }
            if (numTrack > 3 && (numTrack - 3) < 10)
            {
                orderNumText.text = "D.0" + (numTrack - 3);
            }
            if ((numTrack - 3) >= 10)
            {
                orderNumText.text = "D." + (numTrack - 3);
            }
        }
        if (track.CompareTag("SpriteOBJ"))
        {
            if ((numTrack + 1) < 10)
            {
                orderNumText.text = "A.00" + (numTrack + 1);
            }
            if ((numTrack + 1) >= 10 && (numTrack + 1) < 100)
            {
                orderNumText.text = "A.0" + (numTrack + 1);
            }
            if ((numTrack + 1) >= 100)
            {
                orderNumText.text = "A." + (numTrack + 1);
            }
        }
    }

    void startAnimator()
    {
        transform.GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        anim.SetInteger("state", 1);
    }

    public void StartAnimTimer()
    {
        pressTime = Time.time;
        isPressAnim = true;
    }

    IEnumerator demoonclick()
    {

        yield return new WaitForSeconds(1);
        FindObjectOfType<hamburgerMenuScript>().OnClick();
        track.GetComponent<TrackInfoScript>().ShowUI();

    }

    // Update is called once per frame
    void Update()
    {
        calc = CalculateLengthOfMessage(titleText);

        if (isAudio)
        {

            timeText.rectTransform.anchoredPosition = new Vector2(115f + calc, 0f);
            var v = CalculateLengthOfMessage(timeText);
        }



        if (isPressAnim)
        {
            float width;
            float t = (Time.time - pressTime) / duration;
            if (isSelected)
            {
                if (t >= 0.2f)
                {
                    transform.GetChild(1).GetComponent<Text>().color = Color.black;
                }
                if (t >= 0.7f)
                {
                    transform.GetChild(3).GetComponent<Text>().color = Color.black;
                }

                width = Mathf.SmoothStep(rH.x, buttonWidth, t);
            }
            else
            {
                if (t >= 0.5f)
                {
                    transform.GetChild(1).GetComponent<Text>().color = Color.white;

                }
                if (t >= 0.1f)
                {
                    transform.GetChild(3).GetComponent<Text>().color = Color.white;
                }
                width = Mathf.SmoothStep(buttonWidth, rH.x, t);
            }

            transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(width, 29f);

            if (t >= 1f)
            {
                if (!isSelected)
                {
                    transform.GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
                    transform.GetChild(0).gameObject.GetComponent<buttonWhiteBGscript>().endAnim();
                }
                isPressAnim = false;
            }

        }

    }

    int CalculateLengthOfMessage(Text theText)
    {
        int totalLength = 0;

        Font myFont = theText.font;  //chatText is my Text component
        CharacterInfo characterInfo = new CharacterInfo();

        char[] arr = theText.text.ToCharArray();

        foreach (char c in arr)
        {
            myFont.GetCharacterInfo(c, out characterInfo, theText.fontSize);

            totalLength += characterInfo.advance;
        }

        return totalLength;
    }

    void OnClick()
    {

        if (!isSelected)
        {

            isSelected = true;
            startAnimator();
            var buttons = FindObjectsOfType<trackListButtonScript>();
            foreach (var t in buttons)
            {
                if (t != this)
                {
                    if (t.isSelected)
                    {
                        t.isSelected = false;
                        t.pressTime = Time.time;
                        t.isPressAnim = true;
                    }
                }
            }
        }
        //else
        //{
        //    isSelected = false;
        //    pressTime = Time.time;
        //    isPressAnim = true;

        //}
        if (isAudio)
        {
            buttonWidth = (float)(50 + 15 + 15 + calc + CalculateLengthOfMessage(timeText));
        }
        else
        {
            buttonWidth = (float)(50 + 15 + 15 + calc); //CalculateLengthOfMessage(timeText));
        }

        //isPressAnim = true;
        //pressTime = Time.time;

        if (track.CompareTag("AudioTrack"))
        {
            camTrans = track.transform.GetChild(0).GetChild(0);
        }
        //if (track.CompareTag("SpriteOBJ"))
        //{
        //    camTrans = track.transform.GetChild(0);
        //}
        if (!mo.inACategory)// && track.CompareTag("AudioTrack"))
        {

            mo.lastCamTrans = mo.camTrans;

            mo.camTrans = camTrans;

            if (mo.camTrans != mo.lastCamTrans)
            {
                if (track.CompareTag("AudioTrack"))
                {
                    track.GetComponent<SoundWave>().isTimerRunning = true;
                    track.GetComponent<SoundWave>().isStartingTrail = true;
                    track.GetComponent<SoundWave>().startTime = Time.time;
                    //hit.transform.parent.gameObject.GetComponent<SoundWave>().onMe = true;

                }

                if (mo.lastCamTrans != null && mo.lastCamTrans.root.gameObject.tag == "AudioTrack")
                {
                    mo.lastCamTrans.transform.root.gameObject.GetComponent<SoundWave>().isTimerRunning = true;
                    mo.lastCamTrans.transform.root.gameObject.GetComponent<SoundWave>().isStopingTrail = true;
                    mo.lastCamTrans.transform.root.gameObject.GetComponent<SoundWave>().startTime = Time.time;
                    mo.lastCamTrans.transform.root.gameObject.GetComponent<SoundWave>().onMe = false;
                }

                mo.cameraStartTime = Time.time;
                mo.cameraMoving = true;
                mo.inAudio = true;
            }
        }

        StartCoroutine(demoonclick());

        //transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(400f, rH);
        Debug.Log("clicked!");
    }
}
