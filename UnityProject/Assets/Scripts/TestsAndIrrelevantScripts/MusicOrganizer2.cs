using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MusicOrganizer2 : MonoBehaviour
{

    GameObject parent;
    public Material mat;
    public GameObject circleOBJ;
    public spiralCurve spiral;
    AudioClip[] audiotracks;
    public Sprite[] voyagerImages;

    public List<GameObject> circles = new List<GameObject>();
    List<GameObject> spriteObjects = new List<GameObject>();




    // Use this for initialization
    void Start()
    {
        parent = new GameObject("galaxy");

        voyagerImages = Resources.LoadAll("ImagesVoyager", typeof(Sprite)).Cast<Sprite>().ToArray();
        audiotracks = Resources.LoadAll("Audio", typeof(AudioClip)).Cast<AudioClip>().ToArray();

        for (int i = 0; i < audiotracks.Length; i++)
        {
            GameObject audioGO = new GameObject("track " + (i + 1));
            AudioSource goas = audioGO.AddComponent<AudioSource>();
            goas.playOnAwake = false;
            goas.clip = audiotracks[i];

            CreateAudioTrack(audioGO, i);
            audioGO.transform.parent = parent.transform;
        }

        for (int i = 0; i < voyagerImages.Length; i++)
        {
            GameObject spriteOBJ = new GameObject("image" + (i + 1));
            SpriteRenderer sr = spriteOBJ.AddComponent<SpriteRenderer>();
            sr.sprite = voyagerImages[i];


            spriteObjects.Add(spriteOBJ);
            spriteOBJ.transform.parent = parent.transform;

        }
        PutImagesOnSpline();
    }

    void PutImagesOnSpline()
    {
        for (int i = 0; i < spriteObjects.Count; i++)
        {
            float step = Mathf.Lerp(0f, 0.18f, (float)i / (float)voyagerImages.Length);
            Vector3 pointOnSpiral = spiral.spiralGraph(step);
            spriteObjects[i].transform.position = pointOnSpiral;

            //Vector3 targetDeg = Vector3.zero - pointOnSpiral;
            //Quaternion rotation = Quaternion.LookRotation(targetDeg, Vector3.up);
            //spriteObjects[i].transform.rotation = rotation;
        }
    }

    void CreateAudioTrack(GameObject go, int num)
    {

        List<GameObject> tempCircles = new List<GameObject>();
        AudioSource aud = go.GetComponent<AudioSource>();

        float[] samples = new float[aud.clip.samples];
        List<float> data = new List<float>();

        aud.clip.GetData(samples, 0);

        int numOfData = samples.Length / 900;


        for (int i = 0; i < samples.Length; i += numOfData)
        {

            data.Add(samples[i]);
        }


        for (int i = 0; i < data.Count; i++)
        {

            //if (data[i] > 0f)
            //{


            GameObject circle = GameObject.CreatePrimitive(PrimitiveType.Cube); //Instantiate<GameObject>(circleOBJ);
            circle.GetComponent<Renderer>().material = mat;
            //GameObject orbit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //orbit.name = "orbit " + i;
            //TrailRenderer tr = orbit.AddComponent<TrailRenderer>();
            //tr.startWidth = 0.3f;
            //tr.endWidth = 0f;
            //tr.time = 9f;
            //tr.material = mat;
            //tr.enabled = false;

            //float randomSize = Random.Range(0.8f, 2.5f);
            //Vector3 oSize = new Vector3(randomSize, randomSize, randomSize);
            //orbit.transform.localScale = oSize;

            //orbit.GetComponent<Renderer>().material = mat;
            //walker wCode = orbit.AddComponent(typeof(walker)) as walker;
            //wCode.spline = circle.GetComponent<BezierSpline>();
            //wCode.duration = Random.Range(6f, 20f);
            ////wCode.progress = Random.Range(0f, 1f);

            circle.transform.parent = go.transform;
            //orbit.transform.parent = go.transform;

            circle.transform.localScale = new Vector3(1f, 300f * (Mathf.Clamp01(data[i])), 1f);

            circles.Add(circle);
            tempCircles.Add(circle);

            //SoundWave sw = go.AddComponent<SoundWave>();
            //sw.circleObjects.Add(circle);

            //}
        }
        for (int i = 0; i < tempCircles.Count; i++)
        {

            float step = Mathf.Lerp((float)num / (float)audiotracks.Length, (float)(num + 1) / (float)audiotracks.Length, 0.93f * ((float)i / (float)tempCircles.Count));
            float afterPicsStep = Mathf.Lerp(0.18f, 1f, step);
            Vector3 pointOnSpiral = spiral.spiralGraph(afterPicsStep);
            tempCircles[i].transform.position = pointOnSpiral;

            Vector3 targetDeg = pointOnSpiral - Vector3.zero;
            Quaternion rotation = Quaternion.LookRotation(targetDeg);
            tempCircles[i].transform.rotation = rotation;
        }


    }

    private void Update()
    {
        //parent.transform.Rotate(Vector3.up * Time.deltaTime);
        for (int i = 0; i < spriteObjects.Count; i++)
        {
            spriteObjects[i].transform.LookAt(Camera.main.transform);
        }

    }

}
