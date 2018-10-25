using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class MusicOrganizer : MonoBehaviour
{
    #region start variables
    public List<GameObject> catNames = new List<GameObject>();

    //play and stop sprites
    public Sprite s_play, s_stop;

    //ui elements
    public GameObject buttonPrefab;
    public GameObject uiPanelA, uiPanelB, uiPanelC, uiPanelD, uiPanel_photos, uiPanel_audio, gradD, gradU;

    public GameObject ballSplineStartAnim;

    //variables swipe or zoom control
    bool swipeUp, swipeDown, swipeLeft, swipeRight, isDragging;
    Vector2 startTouch, swipeDelta;

    float previousDistance;
    float zoomSpeed = 0.1f;
    float lerpMaxCamY = 0f;
    float lerpMinCamY = 0f;
    float maxZCam = -1500f;
    float minZCam = -5000f;
    float maxYCam = 2000f;
    float minYCam = -2000f;

    //variables for mechanisem for rotating categories
    bool longTap = false;
    float taptimer = 0.1f;
    float tapStartTime, _sensitivity;
    float _ud_sensitivity = 1.5f;
    private Vector3 _mouseReference, _mouseOffset, _rotation;
    public bool _isRotating, _isGoingUpOrDown;

    public bool inImage, inAudio, inCategories, inACategory;// = false;
    public GameObject ballSpiralOBJ;

    public Material mat;
    public GameObject circleOBJ;
    public spiralCurve spiral;
    public spiralCurve openedSpiral;
    List<AudioClip> audiotracks;
    List<Sprite> voyagerImages;

    float pointOfAudioTracks = 0.2f;
    float maxTrackWidth = 0.9f;

    float startTime;
    float duration = 5f;

    public Transform camTrans;
    public Transform lastCamTrans;

    public bool cameraMoving = false;
    float cameraDuration = 5f;
    public float cameraStartTime;
    float cameraTimer;
    public GameObject camOrigin;
    public GameObject catCamOrigin;
    GameObject currentCat;
    public GameObject catPodium;

    CategoryOrganizer catMasterCode;
    List<GameObject> catSpheres = new List<GameObject>();

    public List<GameObject> tracks = new List<GameObject>();
    List<GameObject> middlepoints = new List<GameObject>();
    List<GameObject> listOfObjects = new List<GameObject>();
    List<GameObject> spriteObjects = new List<GameObject>();

    public List<string> tracknames = new List<string>();
    public List<string> imageNames = new List<string>();

    float timeTillOpen = 7f;
    bool animStart = true;
    public bool isSpiralOpening;// = true;
    float timer;


    public bool firstAnim = true;
    #endregion

    private void Awake()
    {
        _sensitivity = 0.15f;
        _rotation = Vector3.zero;

        GameObject catMaster = new GameObject("categoryMaster");
        catMasterCode = catMaster.AddComponent<CategoryOrganizer>();

        camOrigin = new GameObject("camOrigin");
        camOrigin.transform.position = new Vector3(0, 0f, -5000f);
        camOrigin.transform.rotation = Quaternion.identity;

        catCamOrigin = new GameObject("catCamOrigin");
        catCamOrigin.transform.position = new Vector3(0f, 0f, -2000f);
        catCamOrigin.transform.eulerAngles = Vector3.zero;

        startTime = Time.time;

        voyagerImages = FindObjectOfType<AssetLoaderHolder>().sprites;
        audiotracks = FindObjectOfType<AssetLoaderHolder>().audioClips;

        imageNames = LoadTrackAndImageNames(true);
        tracknames = LoadTrackAndImageNames(false);

        for (int i = 0; i < voyagerImages.Count; i++)
        {
            GameObject spriteOBJ = new GameObject("image" + (i + 1));
            spriteOBJ.AddComponent<objectMover>();

            SpriteRenderer sr = spriteOBJ.AddComponent<SpriteRenderer>();
            sr.sprite = voyagerImages[i];
            spriteOBJ.tag = "SpriteOBJ";
            spriteOBJ.AddComponent<BoxCollider>();

            spriteOBJ.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);

            var tis = spriteOBJ.AddComponent<TrackInfoScript>();
            tis.myUiObject = uiPanel_photos.transform.GetChild(i).gameObject;

            spriteObjects.Add(spriteOBJ);
            tracks.Add(spriteOBJ);
            listOfObjects.Add(spriteOBJ);
            giveCategory(spriteOBJ);

        }
        for (int i = 0; i < audiotracks.Count; i++)
        {
            GameObject audioGO = new GameObject("track " + (i + 1));
            audioGO.tag = "AudioTrack";

            AudioSource goas = audioGO.AddComponent<AudioSource>();
            goas.playOnAwake = false;
            goas.clip = audiotracks[i];

            var gosw = audioGO.AddComponent<SoundWave>();
            var tis = audioGO.AddComponent<TrackInfoScript>();
            tis.myUiObject = uiPanel_audio.transform.GetChild(i).gameObject;
            var pbs = uiPanel_audio.transform.GetChild(i).GetChild(2).GetChild(3).gameObject.AddComponent<playButtonScript>();
            pbs.s_play = s_play;
            pbs.s_stop = s_stop;
            pbs.mySW = gosw;

            CreateAudioTrack(audioGO, i);
            giveCategory(audioGO);
            tracks.Add(audioGO);
        }


        PutImagesOnSpline();
        takeCareOfMid();
    }

    void Start()
    {
        categoryParentMaker();
        for (int i = 0; i < uiPanel_audio.transform.childCount; i++)
        {
            var gradDCopy = Instantiate<GameObject>(gradD);
            var gradUCopy = Instantiate<GameObject>(gradU);
            gradDCopy.transform.SetParent(uiPanel_audio.transform.GetChild(i));
            gradUCopy.transform.SetParent(uiPanel_audio.transform.GetChild(i));

        }


    }

    void categoryParentMaker()
    {
        for (int i = 0; i < catMasterCode.categories.Count; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "category " + i;
            sphere.tag = "CatSphere";
            catSpheres.Add(sphere);
            catNames[i].transform.SetParent(sphere.transform);
            catNames[i].transform.localPosition = new Vector3(0f, -0.73f, 0f);
            catNames[i].transform.localScale = new Vector3(0.22f, 0.22f, 0.22f);
            catNames[i].SetActive(true);
            sphere.transform.localScale = new Vector3(15f * catMasterCode.categories[i].Count, 15f * catMasterCode.categories[i].Count, 15f * catMasterCode.categories[i].Count);
            Vector3 spherePos = new Vector3(-1200f + ((float)i * 600f), 0f, 0f);
            sphere.transform.position = spherePos;
            sphere.GetComponent<Renderer>().enabled = false;

            GameObject catMid = new GameObject("camTrans");
            //catMid.name = "camTrans";
            catMid.transform.SetParent(sphere.transform);
            catMid.transform.localPosition = new Vector3(0f, 0f, -1.1f);



            GameObject secondCamTrans = new GameObject("secondCamTrans");
            secondCamTrans.transform.SetParent(sphere.transform);
            secondCamTrans.transform.localPosition = new Vector3(0f, 0f, -2.1f);
            secondCamTrans.transform.SetParent(null, true);
            secondCamTrans.transform.SetParent(catMid.transform, true);

            GameObject objectPodium = Instantiate<GameObject>(catPodium);
            objectPodium.transform.SetParent(sphere.transform);
            objectPodium.transform.localPosition = new Vector3(0f, 0f, -1.1f);

            //creating a live renderer object to circle around the category
            GameObject lineOBJ = Instantiate<GameObject>(circleOBJ);
            BezierSpline circleSpline = lineOBJ.GetComponent<BezierSpline>();
            LineRenderer lr = lineOBJ.AddComponent<LineRenderer>();
            int numoflines = 50;
            lr.positionCount = numoflines;
            lr.widthMultiplier = 3f;
            lr.numCornerVertices = 90;
            lr.numCapVertices = 90;
            lr.material = mat;
            lineOBJ.transform.SetParent(sphere.transform);
            lineOBJ.transform.localPosition = Vector3.zero;
            lineOBJ.transform.localScale = new Vector3(0.28f, 0.28f, 0.28f);
            lineOBJ.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
            for (int b = 0; b < numoflines; b++)
            {
                Vector3 linePos = circleSpline.GetPoint((float)b / (float)(numoflines - 1));
                lr.SetPosition(b, linePos);
            }

            GameObject catBall = Instantiate<GameObject>(ballSpiralOBJ);
            //var catBallGraph = catBall.GetComponent<ballSpiral>();
            catBall.transform.SetParent(sphere.transform);
            catBall.transform.localPosition = Vector3.zero;
            catBall.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);

            GameObject objsParent = new GameObject("objsParent");
            objsParent.transform.SetParent(sphere.transform);
            objsParent.transform.localPosition = Vector3.zero;

            catNames[i].transform.SetAsLastSibling();
            sphere.SetActive(false);
        }
    }

    void giveCategory(GameObject g)
    {
        int catNum = Random.Range(1, 6);
        if (catNum == 1)
        {
            catMasterCode.cat1.Add(g);
        }
        if (catNum == 2)
        {
            catMasterCode.cat2.Add(g);
        }
        if (catNum == 3)
        {
            catMasterCode.cat3.Add(g);
        }
        if (catNum == 4)
        {
            catMasterCode.cat4.Add(g);
        }
        if (catNum == 5)
        {
            catMasterCode.cat5.Add(g);
        }
    }

    void PutImagesOnSpline()
    {
        for (int i = 0; i < spriteObjects.Count; i++)
        {
            float step = Mathf.Lerp(0f, pointOfAudioTracks, (float)i / (float)voyagerImages.Count);

            MyPointOnGraph mpog = spriteObjects[i].AddComponent<MyPointOnGraph>();
            mpog.point = step;

            Vector3 pointOnSpiral = spiral.spiralGraph(step);
            spriteObjects[i].transform.position = pointOnSpiral;

            Vector3 targetDeg = Vector3.zero - pointOnSpiral;
            Quaternion rotation = Quaternion.LookRotation(targetDeg, Vector3.up);
            spriteObjects[i].transform.rotation = rotation;


            GameObject midPoint = new GameObject("camTrans");
            midPoint.tag = "camTrans";
            midPoint.transform.parent = spriteObjects[i].transform;
            midPoint.transform.localRotation = Quaternion.identity;
            midPoint.transform.localPosition = new Vector3(0f, 0f, -140f);

            GameObject newButton = Instantiate<GameObject>(buttonPrefab);
            newButton.GetComponent<trackListButtonScript>().track = spriteObjects[i];
            newButton.transform.GetChild(1).GetComponent<Text>().text = spriteObjects[i].name;
            newButton.GetComponent<trackListButtonScript>().numTrack = i;
            newButton.transform.SetParent(uiPanelA.transform.GetChild(0));
            newButton.GetComponent<trackListButtonScript>().camTrans = midPoint.transform;


            if ((i + 1) < 10)
            {
                spriteObjects[i].GetComponent<TrackInfoScript>().myUiObject.transform.GetChild(2).GetChild(2).gameObject.GetComponent<Text>().text = "A.00" + (i + 1);
            }
            if ((i + 1) >= 10 && (i + 1) < 100)
            {
                spriteObjects[i].GetComponent<TrackInfoScript>().myUiObject.transform.GetChild(2).GetChild(2).gameObject.GetComponent<Text>().text = "A.0" + (i + 1);
            }
            if ((i + 1) >= 100)
            {
                spriteObjects[i].GetComponent<TrackInfoScript>().myUiObject.transform.GetChild(2).GetChild(2).gameObject.GetComponent<Text>().text = "A." + (i + 1);
            }
        }
    }

    void CreateAudioTrack(GameObject go, int num)
    {
        SoundWave sw = go.GetComponent<SoundWave>();
        List<GameObject> tempCircles = new List<GameObject>();
        List<GameObject> tempOrbits = new List<GameObject>();

        AudioSource aud = go.GetComponent<AudioSource>();

        GameObject newButton = Instantiate<GameObject>(buttonPrefab);
        newButton.transform.GetChild(1).GetComponent<Text>().text = go.name;
        if (num <= 2)
        {
            newButton.transform.SetParent(uiPanelB.transform.GetChild(0));
        }
        if (num == 3)
        {
            newButton.transform.SetParent(uiPanelC.transform.GetChild(0));
        }
        if (num > 3)
        {
            newButton.transform.SetParent(uiPanelD.transform.GetChild(0));
        }

        newButton.GetComponent<trackListButtonScript>().track = go;

        string minutes = Mathf.Floor(aud.clip.length / 60).ToString("00");
        string seconds = (aud.clip.length % 60).ToString("00");
        string clipLength = "(" + minutes + ":" + seconds + ")";
        newButton.GetComponent<trackListButtonScript>().trackLength = clipLength;
        newButton.GetComponent<trackListButtonScript>().numTrack = num;

        if (num < 3)
        {
            go.GetComponent<TrackInfoScript>().myUiObject.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = "B.0" + (num + 1);
        }
        if (num == 3)
        {
            go.GetComponent<TrackInfoScript>().myUiObject.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = "C.01";
        }
        if (num > 3 && (num - 3) < 10)
        {
            go.GetComponent<TrackInfoScript>().myUiObject.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = "D.0" + (num - 3);
        }
        if ((num - 3) >= 10)
        {
            go.GetComponent<TrackInfoScript>().myUiObject.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = "D." + (num - 3);
        }
        go.GetComponent<TrackInfoScript>().myUiObject.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = clipLength;


        // all this section used to make sound-waves objects out of the tracks, now converted to JSON file. saving for future reference

        //float[] samples = new float[aud.clip.samples];
        List<float> data = new List<float>();

        //aud.clip.GetData(samples, 0);

        //int numOfData = samples.Length / 950;
        //if (num == 3)
        //{
        //    numOfData = samples.Length / 1440;
        //}

        //for (int i = 0; i < samples.Length; i += numOfData)
        //{

        //    data.Add(samples[i]);
        //}

        //SaveTrackData(data, num);

        data = LoadTrackData(num);

        for (int i = 0; i < data.Count; i++)
        {

            if (data[i] > 0f)
            {


                GameObject circle = Instantiate<GameObject>(circleOBJ);
                GameObject orbit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //orbit.AddComponent<dontShowIf>();
                orbit.name = "orbit " + i;

                orbit.GetComponent<SphereCollider>().radius = 16f;

                TrailRenderer tr = orbit.AddComponent<TrailRenderer>();
                tr.startWidth = 0.6f;
                tr.endWidth = 0f;
                tr.time = 9f;
                tr.material = mat;
                tr.enabled = false;

                float randomSize = Random.Range(0.8f, 3.5f);
                Vector3 oSize = new Vector3(randomSize, randomSize, randomSize);
                orbit.transform.localScale = oSize;

                orbit.GetComponent<Renderer>().material = mat;
                walker wCode = orbit.AddComponent<walker>();
                wCode.spline = circle.GetComponent<BezierSpline>();
                wCode.duration = Random.Range(6f, 20f);
                //wCode.progress = Random.Range(0f, 1f);

                circle.transform.parent = go.transform;
                orbit.transform.parent = go.transform;

                circle.AddComponent<objectMover>();

                if (num < 4)
                {
                    circle.transform.localScale = new Vector3(200f * (Mathf.Clamp01(data[i])), 200f * (Mathf.Clamp01(data[i])), 200f * (Mathf.Clamp01(data[i])));
                }
                else
                {
                    circle.transform.localScale = new Vector3(130f * (Mathf.Clamp01(data[i])), 130f * (Mathf.Clamp01(data[i])), 130f * (Mathf.Clamp01(data[i])));
                }

                listOfObjects.Add(circle);
                tempCircles.Add(circle);
                tempOrbits.Add(orbit);

                orbit.AddComponent<OrbitPartOfTrack>();
                circle.AddComponent<CirclePartOfTrack>();

            }

        }
        for (int i = 0; i < tempCircles.Count; i++)
        {

            float step = Mathf.Lerp((float)num / (float)audiotracks.Count, (float)(num + 1) / (float)audiotracks.Count, maxTrackWidth * ((float)i / (float)tempCircles.Count));
            float afterPicsStep = Mathf.Lerp(pointOfAudioTracks, 1f, step);

            Vector3 pointOnSpiral = spiral.spiralGraph(afterPicsStep);// go.transform.InverseTransformPoint(spiral.spiralGraph(afterPicsStep));

            MyPointOnGraph mpog = tempCircles[i].AddComponent<MyPointOnGraph>();
            mpog.point = afterPicsStep;

            tempCircles[i].transform.position = pointOnSpiral;

            Vector3 targetDeg = pointOnSpiral - Vector3.zero; //go.transform.TransformPoint(pointOnSpiral) - Vector3.zero;
            Quaternion rotation = Quaternion.LookRotation(targetDeg, Vector3.left);
            tempCircles[i].transform.rotation = rotation;

            if (i == (tempCircles.Count / 2))
            {
                //print("hey");
                GameObject midPoint = new GameObject("midPoint");
                MyPointOnGraph mpog_mid = midPoint.AddComponent<MyPointOnGraph>();
                mpog_mid.point = mpog.point;
                midPoint.transform.position = openedSpiral.spiralGraph(mpog_mid.point);
                midPoint.transform.rotation = Quaternion.LookRotation(targetDeg, Vector3.up);
                midPoint.transform.parent = go.transform;
                midPoint.transform.SetAsFirstSibling();

                float yrot = tempCircles[i].transform.eulerAngles.y + 180f;
                Vector3 euRot = new Vector3(0f, yrot, 0f);
                midPoint.transform.eulerAngles = euRot;

                middlepoints.Add(midPoint);
                //midPoint.transform.localPosition = new Vector3(0f, 0f, 365f);

                sw.midPoint = midPoint;

            }
        }

        sw.orbitObjects.AddRange(tempOrbits);
        sw.circleObjects.AddRange(tempCircles);

    }

    // midpoints are the object meant for the camera to know where to go
    void takeCareOfMid()
    {
        for (int i = 0; i < middlepoints.Count; i++)
        {
            GameObject midTrans = new GameObject("camTrans");
            midTrans.tag = "camTrans";
            midTrans.transform.parent = middlepoints[i].transform;

            //float midRot = Mathf.Lerp(7f, 18f, (float)i / (float)middlepoints.Count);
            float midSpace = Mathf.Lerp(-600f, -320f, (float)i / (float)middlepoints.Count);
            midTrans.transform.localPosition = new Vector3(0f, 0f, midSpace);
            midTrans.transform.localRotation = Quaternion.identity;
            //camTrans.transform.eulerAngles = new Vector3(0f, 0f, midRot);
        }
    }

    void changeSpiral(float t, spiralCurve spiralCode)
    {

        for (int i = 0; i < listOfObjects.Count; i++)
        {

            Vector3 currentPos = listOfObjects[i].transform.position;
            Vector3 newPos;
            if (listOfObjects[i].CompareTag("SpriteOBJ"))
            {
                newPos = spiralCode.spiralGraph(listOfObjects[i].GetComponent<MyPointOnGraph>().point);
            }
            else
            {
                newPos = listOfObjects[i].transform.root.InverseTransformPoint(spiralCode.spiralGraph(listOfObjects[i].GetComponent<MyPointOnGraph>().point));
            }
            Vector3 move = new Vector3(Mathf.SmoothStep(currentPos.x, newPos.x, t), Mathf.SmoothStep(currentPos.y, newPos.y, t), Mathf.SmoothStep(currentPos.z, newPos.z, t));

            listOfObjects[i].transform.position = move;

        }
        Quaternion currentCamQuat = Camera.main.transform.rotation;
        Camera.main.transform.rotation = Quaternion.Slerp(currentCamQuat, Quaternion.identity, t);

        Vector3 currentCamPos = Camera.main.transform.position;

        Vector3 moveCam = Vector3.Slerp(currentCamPos, camOrigin.transform.position, t);
        Camera.main.transform.position = moveCam;
    }

    void cameraMover(Transform trans)
    {

        cameraTimer = (Time.time - cameraStartTime) / cameraDuration;

        //move camera position
        Vector3 camPosition = Camera.main.transform.position;
        Vector3 camMove = Vector3.Slerp(camPosition, trans.position, cameraTimer);
        Camera.main.transform.position = camMove;

        //move camera rotation
        if (trans.rotation.eulerAngles != Camera.main.transform.rotation.eulerAngles)
        {
            Quaternion camEul = Quaternion.Euler(Camera.main.transform.eulerAngles);
            Quaternion camRotEul = Quaternion.Slerp(camEul, Quaternion.Euler(trans.eulerAngles), cameraTimer);
            Camera.main.transform.rotation = camRotEul;
        }

        if (cameraTimer >= 0.79f)
        {
            if (camTrans != null && camTrans.root.gameObject.CompareTag("AudioTrack"))
            {
                camTrans.root.gameObject.GetComponent<SoundWave>().onMe = true;
            }

            cameraTimer = 1f;
            Camera.main.transform.position = trans.position;
            cameraMoving = false;
        }

    }

    public void CategoryMover()
    {

        if (!inCategories)
        {
            catMasterCode.listsShuffle();
            camTrans = catCamOrigin.transform;
            cameraStartTime = Time.time;
            cameraMoving = true;

            for (int i = 0; i < catMasterCode.categories.Count; i++)
            {
                catSpheres[i].SetActive(true);


                for (int d = 0; d < catMasterCode.categories[i].Count; d++)
                {
                    catMasterCode.categories[i][d].transform.SetParent(catSpheres[i].transform.GetChild(4));

                    var g = catMasterCode.categories[i][d];

                    if (g.CompareTag("AudioTrack"))
                    {
                        var circ = g.GetComponent<SoundWave>().circleObjects;
                        foreach (GameObject c in circ)
                        {
                            var om = c.GetComponent<objectMover>();
                            om.GetOldPos();
                            var catBallCode = catSpheres[i].GetComponentInChildren<ballSpiral>();
                            om.newPos = catBallCode.ballGraph((float)d / (float)catMasterCode.categories[i].Count);

                            om.startTime = Time.time;
                            om.isMoving = true;

                        }

                    }
                    else
                    {
                        var om = g.GetComponent<objectMover>();
                        om.GetOldPos();
                        var catBallCode = catSpheres[i].GetComponentInChildren<ballSpiral>();
                        om.newPos = catBallCode.ballGraph((float)d / (float)catMasterCode.categories[i].Count);
                        om.startTime = Time.time;
                        om.isMoving = true;
                    }
                }
            }
            inCategories = true;
        }
        else
        {

            catMasterCode.listsShuffle();
            camTrans = camOrigin.transform;
            cameraStartTime = Time.time;
            cameraMoving = true;

            for (int i = 0; i < catMasterCode.categories.Count; i++)
            {
                catSpheres[i].transform.GetChild(4).transform.eulerAngles = Vector3.zero;
                catSpheres[i].transform.GetChild(4).DetachChildren();
                catSpheres[i].SetActive(false);

                for (int d = 0; d < catMasterCode.categories[i].Count; d++)
                {
                    var g = catMasterCode.categories[i][d];

                    if (g.CompareTag("AudioTrack"))
                    {
                        var circ = g.GetComponent<SoundWave>().circleObjects;
                        foreach (GameObject c in circ)
                        {
                            var om = c.GetComponent<objectMover>();
                            om.GetOldPos();
                            om.newPos = openedSpiral.spiralGraph(c.GetComponent<MyPointOnGraph>().point);
                            om.startTime = Time.time;
                            om.isMoving = true;

                        }
                    }
                    else
                    {
                        var om = g.GetComponent<objectMover>();
                        om.GetOldPos();
                        om.newPos = openedSpiral.spiralGraph(g.GetComponent<MyPointOnGraph>().point);
                        om.startTime = Time.time;
                        om.isMoving = true;
                    }
                }
            }
            inCategories = false;
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void startanim()
    {
        foreach (var obj in listOfObjects)
        {
            var om = obj.GetComponent<objectMover>();
            //om.GetOldPos();
            om.oldPos = ballSplineStartAnim.GetComponent<ballSpiral>().ballGraph(Random.Range(0f, 1f));
            //var catBallCode = catSpheres[i].GetComponentInChildren<ballSpiral>();
            om.newPos = spiral.spiralGraph(obj.GetComponent<MyPointOnGraph>().point);// transform.position;

            om.startTime = Time.time;
            om.isMoving = true;
        }

    }

    public void LastObjectAudio()
    {
        lastCamTrans.transform.root.gameObject.GetComponent<SoundWave>().isTimerRunning = true;
        lastCamTrans.transform.root.gameObject.GetComponent<SoundWave>().isStopingTrail = true;
        lastCamTrans.transform.root.gameObject.GetComponent<SoundWave>().startTime = Time.time;
        lastCamTrans.transform.root.gameObject.GetComponent<SoundWave>().onMe = false;

        lastCamTrans.transform.root.gameObject.GetComponent<TrackInfoScript>().HideUI();

        //inAudio = false;
    }

    public void ClickedOnAudio(GameObject hit)
    {
        hit.transform.root.gameObject.GetComponent<SoundWave>().isTimerRunning = true;
        hit.transform.root.gameObject.GetComponent<SoundWave>().isStartingTrail = true;
        hit.transform.root.gameObject.GetComponent<SoundWave>().startTime = Time.time;

        cameraStartTime = Time.time;
        cameraMoving = true;
        inAudio = true;
        inImage = false;

        hit.transform.root.gameObject.GetComponent<TrackInfoScript>().ShowUI();
    }

    private void ResetTouches()
    {
        isDragging = false;
        startTouch = Vector2.zero;
        swipeDelta = Vector2.zero;
    }

    void swipeControlMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startTouch = (Vector2)Input.mousePosition;//Input.GetTouch(0).position;
        }

        if (isDragging)
        {
            swipeDelta = ((Vector2)Input.mousePosition) - startTouch;
        }

        if (swipeDelta.magnitude > 125f)
        {

            //which direction?
            var x = swipeDelta.x;
            var y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //left or right?
                if (x < 0f)
                {
                    swipeLeft = true;
                }
                if (x > 0f)
                {
                    swipeRight = true;
                }
            }
            if ((Mathf.Abs(x) < Mathf.Abs(y)))
            {
                // up or down
                if (y < 0f)
                {
                    swipeDown = true;
                }
                if (y > 0f)
                {
                    swipeUp = true;
                }
            }

            ResetTouches();
        }
    }

    void swipeControlTouch()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            isDragging = true;
            startTouch = Input.GetTouch(0).position;
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
        {
            ResetTouches();
        }

        if (isDragging)
        {
            swipeDelta = Input.GetTouch(0).position - startTouch;
        }

        //did we cross the swipe deadzone?
        if (swipeDelta.magnitude > 125f)
        {

            //which direction?
            var x = swipeDelta.x;
            var y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //left or right?
                if (x < 0f)
                {
                    swipeLeft = true;
                }
                if (x > 0f)
                {
                    swipeRight = true;
                }
            }
            if ((Mathf.Abs(x) < Mathf.Abs(y)))
            {
                // up or down
                if (y < 0f)
                {
                    swipeDown = true;
                }
                if (y > 0f)
                {
                    swipeUp = true;
                }
            }

            ResetTouches();
        }
    }

    void pinchControl()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
        {
            //caliberater distance
            previousDistance = (Input.GetTouch(0).position - Input.GetTouch(1).position).sqrMagnitude;
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
        {
            GetCamMaxAndMin();

            Vector2 touch1 = Input.GetTouch(0).position;
            Vector2 touch2 = Input.GetTouch(1).position;

            float distance = (touch1 - touch2).sqrMagnitude;

            //camera based on pinch/zoom
            float pinchAmount = ((previousDistance - distance) * -1f) * zoomSpeed * Time.deltaTime;

            Camera.main.transform.Translate(0f, 0f, pinchAmount);

            ////limits the camera movement
            if (Camera.main.transform.position.z < minZCam)
            {
                var curCamPos = Camera.main.transform.position;
                curCamPos.z = minZCam;
                Camera.main.transform.position = curCamPos;
            }
            if (Camera.main.transform.position.z > maxZCam)
            {
                var curCamPos = Camera.main.transform.position;
                curCamPos.z = maxZCam;
                Camera.main.transform.position = curCamPos;

            }

            previousDistance = distance;

        }
    }

    void GetCamMaxAndMin()
    {

        var t = Mathf.InverseLerp(minZCam, maxZCam, Camera.main.transform.position.z);
        lerpMaxCamY = Mathf.Lerp(0f, maxYCam, t);
        lerpMinCamY = Mathf.Lerp(0f, minYCam, t);

    }

    bool CanSkip(GameObject GO)
    {
        bool skip = true;

        if (GO.CompareTag("AudioTrack") && GO.GetComponent<SoundWave>().isScrubbing)
        {
            skip = false;
        }

        return skip;
    }

    void SkipTrack(int addNum)
    {
        GameObject CurrentGO = camTrans.root.gameObject;
        GameObject nextGO = tracks[(tracks.IndexOf(CurrentGO)) + addNum].gameObject;

        if (CanSkip(CurrentGO))
        {
            if (nextGO.CompareTag("AudioTrack"))
            {
                lastCamTrans = camTrans;
                camTrans = nextGO.transform.GetChild(0).GetChild(0);

                ClickedOnAudio(nextGO);
            }
            else if (nextGO.CompareTag("SpriteOBJ"))
            {
                lastCamTrans = camTrans;
                camTrans = nextGO.transform.GetChild(0);

                nextGO.GetComponent<TrackInfoScript>().ShowUI();
                inAudio = false;
                inImage = true;
            }

            if (lastCamTrans != null && lastCamTrans.root.gameObject.CompareTag("AudioTrack"))
            {
                LastObjectAudio();
            }
            if (lastCamTrans != null && lastCamTrans.root.gameObject.CompareTag("SpriteOBJ"))
            {
                lastCamTrans.transform.root.gameObject.GetComponent<TrackInfoScript>().HideUI();
            }

            cameraStartTime = Time.time;
            cameraMoving = true;
        }

    }

    private void Update()
    {
        swipeUp = swipeDown = swipeLeft = swipeRight = false;

        //if (inCategories || inACategory)
        //{
        //    FindObjectOfType<hamburgerMenuScript>().gameObject.SetActive(false);
        //}
        //else
        //{
        //    FindObjectOfType<hamburgerMenuScript>().gameObject.SetActive(true);
        //}

        if (Input.GetMouseButton(0))
        {
            //RaycastHit hit;
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            //{
            //    if (!hit.transform.parent.gameObject.CompareTag("AudioTrack"))
            //    {
            swipeControlMouse();
            //    }
            //}
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ResetTouches();
        }

        //if (Input.touchCount > 0) // && !mo.cameraMoving && !mo.firstAnim && !mo.isSpiralOpening)
        //{

        //    //if (Input.touchCount == 1)
        //    //{
        //    //    swipeControlTouch();
        //    //}

        if (Input.touchCount == 2 && !inAudio && !inImage && !inCategories && !firstAnim && !isSpiralOpening) //!cameraMoving &&)
        {
            _isGoingUpOrDown = false;
            pinchControl();
        }
        //}


        //if (inImage)
        //{
        //    //print(camTrans.root.name);
        //    //print(tracks.IndexOf(camTrans.root.gameObject));
        //}

        if (swipeUp)
        {
            print("swipeUP");
        }
        if (swipeDown)
        {
            print("swipeDown");
        }
        if (swipeRight)
        {
            print("swipeRight");
            SkipTrack(-1);
        }
        if (swipeLeft)
        {
            print("swipeLeft");
            if (inAudio || inImage)
            {
                SkipTrack(1);

            }
        }

        if (animStart)
        {
            startanim();
            animStart = false;
        }

        if (_isRotating)
        {
            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);

            // apply rotation
            _rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;// * -1f;

            // rotate
            currentCat.transform.GetChild(4).transform.Rotate(_rotation);

            // store mouse
            _mouseReference = Input.mousePosition;
        }

        if (_isGoingUpOrDown)
        {
            ////limits the camera movement
            if (Camera.main.transform.position.y < lerpMinCamY)
            {
                var curCamPos = Camera.main.transform.position;
                curCamPos.y = lerpMinCamY;
                Camera.main.transform.position = curCamPos;
            }
            if (Camera.main.transform.position.y > lerpMaxCamY)
            {
                var curCamPos = Camera.main.transform.position;
                curCamPos.y = lerpMaxCamY;
                Camera.main.transform.position = curCamPos;
            }

            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);
            //print(_mouseReference);

            // apply rotation
            _rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _ud_sensitivity;

            // translate, not really rotation just co[ied a code from category rotating
            Camera.main.transform.Translate(_rotation);

            // store mouse
            _mouseReference = Input.mousePosition;

        }

        if (firstAnim)
        {
            timeTillOpen -= Time.deltaTime;
            if (timeTillOpen <= 0f)
            {
                startTime = Time.time;
                isSpiralOpening = true;
                firstAnim = false;
            }

        }

        timer = (Time.time - startTime) / duration;
        if (isSpiralOpening)
        {
            if (timer <= 1f)
            {
                changeSpiral(timer, openedSpiral);

            }
            if (timer > 1f)
            {
                isSpiralOpening = false;
            }
        }

        if (cameraMoving)
        {
            cameraMover(camTrans);
        }

        if (Input.GetMouseButtonDown(0))
        {
            tapStartTime = Time.time;
        }
        if (Input.GetMouseButton(0))
        {

            float timeOfTap = Time.time - tapStartTime;
            if (timeOfTap >= taptimer)
            {
                longTap = true;
            }
        }

        if (longTap && inACategory)
        {
            _isRotating = true;

            // store mouse
            _mouseReference = Input.mousePosition;
        }

        if (longTap && !inCategories && !inACategory && !inAudio && !inImage && !IsPointerOverUIObject())
        {
            _isGoingUpOrDown = true;

            // store mouse
            _mouseReference = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isRotating = false;
            _isGoingUpOrDown = false;

            if (!IsPointerOverUIObject() && !firstAnim && !isSpiralOpening)
            {
                if (!longTap)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider != null)
                        {
                            if (hit.transform.gameObject.CompareTag("SpriteOBJ"))
                            {
                                if (!inACategory)
                                {
                                    lastCamTrans = camTrans;
                                    camTrans = hit.transform.GetChild(0);

                                    if (camTrans != lastCamTrans)
                                    {
                                        hit.transform.gameObject.GetComponent<TrackInfoScript>().ShowUI();

                                        if (lastCamTrans != null && lastCamTrans.root.gameObject.CompareTag("AudioTrack"))
                                        {
                                            LastObjectAudio();
                                        }
                                        if (lastCamTrans != null && lastCamTrans.root.gameObject.CompareTag("SpriteOBJ"))
                                        {
                                            lastCamTrans.transform.root.gameObject.GetComponent<TrackInfoScript>().HideUI();
                                        }
                                    }

                                    cameraStartTime = Time.time;
                                    cameraMoving = true;
                                    inAudio = false;
                                    inImage = true;
                                }

                            }
                            if (hit.transform.parent != null && hit.transform.parent.gameObject.CompareTag("AudioTrack"))
                            {

                                if (!inACategory)
                                {

                                    lastCamTrans = camTrans;
                                    camTrans = hit.transform.parent.GetChild(0).GetChild(0);

                                    if (camTrans != lastCamTrans)
                                    {

                                        ClickedOnAudio(hit.transform.gameObject);

                                        if (lastCamTrans != null && lastCamTrans.root.gameObject.CompareTag("AudioTrack"))
                                        {
                                            LastObjectAudio();
                                        }
                                        if (lastCamTrans != null && lastCamTrans.root.gameObject.CompareTag("SpriteOBJ"))
                                        {
                                            lastCamTrans.transform.root.gameObject.GetComponent<TrackInfoScript>().HideUI();
                                        }


                                    }
                                }

                            }
                            if (hit.transform.gameObject.CompareTag("CatSphere"))
                            {
                                inACategory = true;
                                currentCat = hit.transform.gameObject;
                                currentCat.GetComponent<Collider>().enabled = false;
                                print(currentCat.name);

                                camTrans = hit.transform.GetChild(0);
                                cameraStartTime = Time.time;
                                cameraMoving = true;
                            }

                        }
                    }
                    else
                    {
                        FindObjectOfType<CreditsScript>().gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "i";
                        FindObjectOfType<CreditsScript>().onCredits = false;
                        FindObjectOfType<CreditsScript>().creditsText.SetActive(false);

                        if (!inCategories)
                        {
                            inImage = false;
                            inAudio = false;

                            if (camTrans != null && camTrans != camOrigin.transform)
                            {
                                lastCamTrans = camTrans;
                                camTrans = camOrigin.transform;

                                if (lastCamTrans != null && lastCamTrans.root.gameObject.CompareTag("AudioTrack"))
                                {
                                    LastObjectAudio();

                                }
                                if (lastCamTrans != null && lastCamTrans.root.gameObject.CompareTag("SpriteOBJ"))
                                {
                                    lastCamTrans.transform.root.gameObject.GetComponent<TrackInfoScript>().HideUI();

                                }

                                cameraStartTime = Time.time;
                                cameraMoving = true;
                            }

                        }
                        else
                        {
                            if (camTrans != null && camTrans != catCamOrigin.transform)
                            {
                                lastCamTrans = camTrans;
                                camTrans = catCamOrigin.transform;

                                if (lastCamTrans != null && lastCamTrans.root.gameObject.CompareTag("AudioTrack"))
                                {
                                    LastObjectAudio();
                                }
                                cameraStartTime = Time.time;
                                cameraMoving = true;
                            }
                            if (lastCamTrans.root.gameObject.CompareTag("CatSphere"))
                            {
                                lastCamTrans.root.gameObject.GetComponent<Collider>().enabled = true;
                                inACategory = false;
                            }
                        }
                    }
                }
                else
                {
                    longTap = false;
                }
            }


        }
    }

    List<float> LoadTrackData(int num)
    {
        TextAsset ta = Resources.Load<TextAsset>("jsons/data" + num);
        List<float> newList;
        string dataAsJson = ta.text;
        SerData loadedData = JsonUtility.FromJson<SerData>(dataAsJson);

        newList = loadedData.data;

        return newList;

    }

    List<string> LoadTrackAndImageNames(bool isImages)
    {
        TextAsset ta = Resources.Load<TextAsset>("jsons/imagesAndTrackNames");
        List<string> newList;
        string dataAsJson = ta.text;
        SerData loadedData = JsonUtility.FromJson<SerData>(dataAsJson);

        if (isImages)
        {
            newList = loadedData.imageNames;
        }
        else
        {
            newList = loadedData.tracknames;
        }


        return newList;

    }

    void SaveTrackData(List<float> list, int num)
    {
        string curDataFileName = "data" + num + ".json";
        string filePath = Path.Combine(Application.streamingAssetsPath, curDataFileName);
        SerData sd = new SerData();
        sd.data = list;

        string dataAsJson = JsonUtility.ToJson(sd);
        File.WriteAllText(filePath, dataAsJson);
    }
}
