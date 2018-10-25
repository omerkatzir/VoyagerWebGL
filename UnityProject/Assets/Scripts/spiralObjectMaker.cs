using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiralObjectMaker : MonoBehaviour {

    bool finishedCreatingOBJ = false;
    bool cameraMoving = false;

    Transform camTrans;

    public GameObject cube;
    spiralCurve sc;
    public spiralCurve openSpiralCurve;
    int numOfOBJ = 104+118;
    int objIndex = 1;
    float timer = 0.01f;
    float defaultTimer = 0.01f;
    float delayDuration = 0.65f;
    float startTime;
    float duration = 5f;
    float cameraDuration = 5f;
    float cameraStartTime;
    Vector3 p1;
    GameObject camOrigin;
    public List<GameObject> golist;

	// Use this for initialization
	void Start () {
        camOrigin = new GameObject("camOrigin");
        camOrigin.transform.position = new Vector3(0, -3f, -55f);
        camOrigin.transform.rotation = Quaternion.identity;
            
        golist = new List<GameObject>();
        sc = gameObject.GetComponent<spiralCurve>();

	}
	
	// Update is called once per frame
	void Update () {


        if (!finishedCreatingOBJ){
            animationTimer();   
        }
        if(finishedCreatingOBJ){
            float t = (Time.time - startTime) / duration;
            if (t <= 1f)
            {
                openSpiral(t);
            }    
        }



        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)){

                camTrans = hit.transform.gameObject.GetComponent<sinymover>().camTrans;
                cameraStartTime = Time.time;
                cameraMoving = true;
            }else{
                camTrans = camOrigin.transform;
                cameraStartTime = Time.time;
                cameraMoving = true;
            }
        }

        if (cameraMoving){
            cameraMover(camTrans);
        }
	
    }

    void animationTimer(){
        if (objIndex < numOfOBJ)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0f)
        {
            objCreator();
            timer = defaultTimer;
        }

        if (objIndex >= numOfOBJ)
        {
            delayDuration -= Time.deltaTime;
            if (delayDuration <= 0f)
            {
                startTime = Time.time;
                finishedCreatingOBJ = true;
            }
        }
    }

    void cameraMover(Transform trans){
        
        float t = (Time.time - cameraStartTime) / cameraDuration;

        //move camera position
        Vector3 camPosition = Camera.main.transform.position;
        Vector3 camMove = Vector3.Slerp(camPosition, trans.position, t);
        Camera.main.transform.position = camMove;

        //move camera rotation
        Quaternion camEul = Quaternion.Euler(Camera.main.transform.eulerAngles);
        Quaternion camRotEul = Quaternion.Lerp(camEul, Quaternion.Euler(trans.eulerAngles), t);
        Camera.main.transform.rotation = camRotEul;

        if (t>=1f){
            cameraMoving = false;
        }

    }

    void objCreator(){
        
        Vector3 copyPos = sc.spiralGraph((float)objIndex / (float)numOfOBJ);
        GameObject copy = Instantiate(cube, copyPos, Quaternion.identity);
        Vector3 targetDeg = copy.transform.position - Vector3.zero;
        Quaternion rotation = Quaternion.LookRotation(targetDeg);
        copy.transform.rotation = rotation;
        copy.GetComponent<sinymover>().cubenumber = objIndex;
        golist.Add(copy);
        objIndex++;

    }

    void openSpiral(float t){ 
        
        for (int i = 0; i < golist.Count;i++){
            
            Vector3 currentPos = golist[i].transform.position;
            Vector3 newPos = openSpiralCurve.spiralGraph((float)i / (float)golist.Count);
            Vector3 move = new Vector3(Mathf.SmoothStep(currentPos.x, newPos.x, t), Mathf.SmoothStep(currentPos.y, newPos.y, t), Mathf.SmoothStep(currentPos.z, newPos.z, t));
            golist[i].transform.position = move;

            float yrot = golist[i].transform.eulerAngles.y + 180f;
            Vector3 euRot = new Vector3(0f, yrot, 0f);
            golist[i].GetComponent<sinymover>().camTrans.localPosition = new Vector3(0f, 0f, 10f);
                     
            golist[i].GetComponent<sinymover>().camTrans.eulerAngles = euRot;

        }
        if(!cameraMoving){
            Quaternion currentCamQuat = Camera.main.transform.rotation;
            Camera.main.transform.rotation = Quaternion.Slerp(currentCamQuat, Quaternion.identity, t);

            Vector3 currentCamPos = Camera.main.transform.position;

            Vector3 moveCam = Vector3.Slerp(currentCamPos, camOrigin.transform.position, t);
            Camera.main.transform.position = moveCam;
        }

    }
}
