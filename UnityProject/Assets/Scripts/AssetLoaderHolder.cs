using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoaderHolder : MonoBehaviour
{

    public List<AudioClip> audioClips = new List<AudioClip>();
    public List<Sprite> sprites = new List<Sprite>();

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

}
