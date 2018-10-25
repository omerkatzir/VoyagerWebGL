using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitPartOfTrack : MonoBehaviour
{
    SoundWave tsw;
    public float part;
    TrailRenderer tr;
    bool nowPlaying = false;

    private void Start()
    {
        tsw = transform.parent.gameObject.GetComponent<SoundWave>();
        tr = gameObject.GetComponent<TrailRenderer>();

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
            if (tsw.song_t < part)
            {
                tr.Clear();

            }
        }

    }
}
