using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playButtonScript : MonoBehaviour
{
    public Sprite s_play;
    public Sprite s_stop;

    public SoundWave mySW;
    bool isPlaying;

    // Use this for initialization
    void Start()
    {
        GetComponent<Image>().sprite = s_play;
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (mySW.onMe)
        {
            isPlaying = mySW.aud.isPlaying;
            //if (isPlaying)
            //{
            //    GetComponent<Image>().sprite = s_stop;
            //}
            //else
            //{
            //    GetComponent<Image>().sprite = s_play;
            //}

        }
    }


    void OnClick()
    {
        if (isPlaying)
        {
            mySW.aud.time = 0f;
            mySW.aud.Stop();
            GetComponent<Image>().sprite = s_play;
        }
        else
        {
            mySW.stopAllAudio();
            mySW.aud.Play();
            GetComponent<Image>().sprite = s_stop;
        }
    }
}
