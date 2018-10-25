using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePartOfTrack : MonoBehaviour
{
    SoundWave tsw;
    public float part;
    BezierSpline bs;
    bool nowPlaying = false;
    Vector3 c;


    private void Start()
    {
        tsw = transform.parent.gameObject.GetComponent<SoundWave>();

        bs = gameObject.GetComponent<BezierSpline>();
        c = transform.localScale;

    }

    private void Update()
    {
        if (tsw.aud.isPlaying)
        {
            nowPlaying = true;
        }
        else
        {
            nowPlaying = false;
        }

        if (nowPlaying)
        {

            if (tsw.song_t > part)
            {
                Vector3 size = transform.localScale;
                ChangeSize(size, c);
            }
            if (tsw.song_t < part)
            {
                Vector3 size = transform.localScale;
                ChangeSize(size, Vector3.zero);
            }
        }
        else
        {
            Vector3 size = transform.localScale;
            ChangeSize(size, c);
        }

    }

    void ChangeSize(Vector3 from, Vector3 to)
    {
        transform.localScale = Vector3.Lerp(from, to, Time.deltaTime * 5f);
    }

}
